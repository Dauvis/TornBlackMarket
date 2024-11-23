using Microsoft.Extensions.Logging;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Repositories;

namespace TornBlackMarket.Logic.Services
{
    public class ExchangeService: IExchangeService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ILogger<ExchangeService> _logger;
        private IExchangeRepository? _exchangeRepository;

        public ExchangeService(IRepositoryFactory repositoryFactory, ILogger<ExchangeService> logger)
        {
            _repositoryFactory = repositoryFactory;
            _logger = logger;
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

        public async Task<bool> UpdateAsync(string exchangeId, ExchangeDocumentDTO exchangeDto)
        {
            if (exchangeId != exchangeDto.Id)
            {
                _logger.LogCritical("Illegal attempt to update exchange {ProfileId} by {UserProfileId}", exchangeDto.Id, exchangeId);
                return false;
            }

            var repository = GetExchangeRepository();
            return await repository.UpdateAsync(exchangeDto);
        }
    }
}
