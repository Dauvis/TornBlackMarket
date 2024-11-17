using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TornBlackMarket.Data.Interfaces;

namespace TornBlackMarket.Data.Abstraction
{
    public class DataStoreClient : IDataStoreClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataStoreClient> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly string _baseConnectionString;

        public DataStoreClient(IConfiguration configuration, ILogger<DataStoreClient> logger, IServiceProvider serviceProvider, IMapper mapper)
        {
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _baseConnectionString = _configuration["TBM_CONNECT_INFO"] ?? "";

            if (string.IsNullOrEmpty(_baseConnectionString))
            {
                _logger.LogCritical("Environment variable TBM_CONNECT_INFO does not have a base connection string");
                throw new ArgumentException("Base connection string has not be set", nameof(configuration));
            }
        }

        public IDataStoreCollection GetCollection(string databaseName)
        {
            var collectionLogger = _serviceProvider.GetRequiredService<ILogger<DataStoreCollection>>();            
            var collection = new DataStoreCollection(_baseConnectionString, _configuration, databaseName, collectionLogger, _serviceProvider, _mapper);
            collection.Initialize();

            return collection;
        }
    }
}
