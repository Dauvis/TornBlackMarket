using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface IExchangeRepository
    {
        Task<ExchangeDocumentDTO?> CreateAsync(ExchangeDocumentDTO exchangeDto);
        Task<ExchangeDocumentDTO?> GetAsync(string exchangeId);
    }
}
