using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Attributes;
using TornBlackMarket.Data.Interfaces;
using TornBlackMarket.Data.Repositories;
using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace TornBlackMarket.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private static readonly ConcurrentDictionary<Type, object> _repositoryCache = new();
        private static readonly ConcurrentDictionary<Type, Type> _repositoryImplementationMap = new();

        private readonly IDataStoreClient _dataStoreClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RepositoryFactory> _logger;

        public RepositoryFactory(IDataStoreClient dataStoreClient, IServiceProvider serviceProvider, ILogger<RepositoryFactory> logger)
        {
            _dataStoreClient = dataStoreClient;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public T? Create<T>() where T : class
        {
            if (_repositoryImplementationMap.IsEmpty)
            {
                BuildRepositoryImplementationMap();
            }

            var interfaceType = typeof(T);

            if (_repositoryCache.TryGetValue(interfaceType, out object? value))
            {
                _logger.LogDebug("Using cached repository instance for {InterfaceName}", interfaceType.Name);
                return (T?)value;
            }

            if (!_repositoryImplementationMap.TryGetValue(interfaceType, out var repositoryType))
            {
                _logger.LogCritical("No implementation class for {InterfaceName} found in mappings", interfaceType.Name);
                throw new InvalidOperationException($"No implementation mapping found for {interfaceType.Name}");
            }

            var repositoryAttribute = repositoryType.GetCustomAttribute<DataStoreRepositoryAttribute>();

            if (repositoryAttribute is null)
            {
                _logger.LogCritical("Repository {RepositoryName} lacks required {AttributeName}", repositoryType.Name, nameof(DataStoreRepositoryAttribute));
                throw new InvalidOperationException($"{repositoryType.Name} lacks a {nameof(DataStoreRepositoryAttribute)}");
            }

            var collection = _dataStoreClient.GetCollection(repositoryAttribute.CollectionName);
            var getRepositoryMethod = collection.GetType().GetMethod("GetRepository")!.MakeGenericMethod(repositoryType);
            var repository = getRepositoryMethod.Invoke(collection, null);

            if (repository is not null)
            {
                _logger.LogDebug("Adding instance for {InterfaceName} to repository cache", interfaceType.Name);
                _repositoryCache[interfaceType] = repository;
            }

            return (T?)repository;
        }

        private void BuildRepositoryImplementationMap()
        {
            _logger.LogDebug("Constructing repository interface to implementation mappings");
            _repositoryImplementationMap[typeof(IProfileRepository)] = typeof(UserProfileRepository);
            _repositoryImplementationMap[typeof(IItemRepository)] = typeof(ItemRepository);
        }
    }
}
