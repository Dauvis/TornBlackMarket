using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TornBlackMarket.Data.Interfaces;
using Microsoft.Data.SqlClient;

namespace TornBlackMarket.Data.Abstraction
{
    public class DataStoreCollection : IDataStoreCollection
    {
        private readonly string _baseConnectionString;
        private readonly IConfiguration _configuration;
        private readonly string _databaseName;
        private readonly ILogger<DataStoreCollection> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly string _databaseId;
        private SqlConnection? _database;

        public DataStoreCollection(string baseConnectionString, IConfiguration configuration, string databaseName, 
            ILogger<DataStoreCollection> logger, IServiceProvider serviceProvider, IMapper mapper)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                logger.LogCritical("Name of collection has not been specified");
                throw new ArgumentException("Name of collection has not been specified", nameof(databaseName));
            }

            _baseConnectionString = baseConnectionString;
            _configuration = configuration;
            _databaseName = databaseName;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _databaseId = _configuration[$"TornBlackMarket:Databases:{databaseName}"] ?? "";

            if (string.IsNullOrEmpty(_databaseId))
            {
                logger.LogCritical("No database identifier found for TornBlackMarket:Databases:{CollectionName}", databaseName);
                throw new ArgumentException($"No database identifier found for TornBlackMarket:Databass:{databaseName}");
            }
        }

        public void Initialize()
        {
            _database = new($"{_baseConnectionString}Initial Catalog={_databaseId};");
            _logger.LogDebug("Connection string for {DatabaseName}: {ConnectionString}", _databaseName, _database);
        }

        public DataStoreRepository<T> GetRepository<T>() where T : DataStoreRepository<T>
        {
            var repositoryType = typeof(T);

            if (!repositoryType.IsSubclassOf(typeof(DataStoreRepository<T>)))
            {
                _logger.LogCritical("{RepositoryType} is not a supported class for repositories", repositoryType.Name);
                throw new InvalidOperationException($"{repositoryType.Name} is not a supported class for repositories");
            }

            if (_database is null)
            {
                _logger.LogCritical("{DataStoreName} for collection named {CollectionName} has not been initialized", 
                    nameof(DataStoreCollection), _databaseName);
                throw new InvalidOperationException($"{nameof(DataStoreCollection)} for {_databaseName} has not been initialized");
            }

            var repositoryLogger = _serviceProvider.GetRequiredService<ILogger<T>>();
            object[] ctorArgs = [_database, repositoryLogger, _serviceProvider, _mapper, _configuration];
            var repository = (DataStoreRepository<T>?)Activator.CreateInstance(repositoryType, ctorArgs);

            if (repository is null)
            {
                _logger.LogCritical("General failure to instantiate repository class {ClassName}", repositoryType.Name);
                throw new InvalidOperationException($"Failed to instantiate {repositoryType.Name}");
            }

            repository.Initialize();

            return repository;
        }
    }
}
