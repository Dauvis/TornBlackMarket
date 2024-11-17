
namespace TornBlackMarket.Common.Interfaces
{
    public interface IRepositoryFactory
    {
        T? Create<T>() where T : class;
    }
}
