using TornBlackMarket.Data.Abstraction;

namespace TornBlackMarket.Data.Interfaces
{
    public interface IDataStoreCollection
    {
        DataStoreRepository<T> GetRepository<T>() where T : DataStoreRepository<T>;
    }
}
