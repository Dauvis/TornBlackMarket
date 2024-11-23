using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Repositories;

namespace TornBlackMarket.Logic.Services
{
    public class ExchangeService: IExchangeService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private IExchangeRepository? _exchangeRepository;

        public ExchangeService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        protected IExchangeRepository GetExchangeRepository()
        {
            _exchangeRepository ??= _repositoryFactory.Create<IExchangeRepository>()
                    ?? throw new InvalidOperationException($"Failed to instantiate repository: {nameof(IExchangeRepository)}");

            return _exchangeRepository;
        }

        public async Task<ExchangeDocumentDTO?> CreateAsync(ExchangeDocumentDTO exchange)
        {
            var repository = GetExchangeRepository();
            return await repository.CreateAsync(exchange);
        }

        public async Task<ExchangeDocumentDTO?> GetAsync(string exchangeId)
        {
            var repository = GetExchangeRepository();
            return await repository.GetAsync(exchangeId);
        }
    }
}
