namespace TornBlackMarket.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataStoreRepositoryAttribute : Attribute
    {
        public DataStoreRepositoryAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public string CollectionName { get; }
    }
}
