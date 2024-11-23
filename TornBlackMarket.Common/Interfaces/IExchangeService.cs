using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface IExchangeService
    {
        Task<ExchangeDocumentDTO?> CreateAsync(ExchangeDocumentDTO exchange);
        Task<ExchangeDocumentDTO?> GetAsync(string exchangeId);
        Task<bool> UpdateAsync(string exchangeId, ExchangeDocumentDTO exchangeDto);
    }
}
