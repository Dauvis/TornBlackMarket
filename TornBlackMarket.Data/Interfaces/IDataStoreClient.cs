namespace TornBlackMarket.Data.Interfaces
{
    public interface IDataStoreClient
    {
        IDataStoreCollection GetCollection(string databaseName);
    }
}
