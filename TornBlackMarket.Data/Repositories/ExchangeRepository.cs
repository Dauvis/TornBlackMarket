using AutoMapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Abstraction;
using TornBlackMarket.Data.Attributes;
using TornBlackMarket.Data.Models;

namespace TornBlackMarket.Data.Repositories
{
    [DataStoreRepository("Main")]
    public class ExchangeRepository : DataStoreRepository<ExchangeRepository>, IExchangeRepository
    {
        public ExchangeRepository(SqlConnection database, ILogger<ExchangeRepository> logger, IServiceProvider serviceProvider, 
            IMapper mapper, IConfiguration configuration) : 
            base(database, logger, serviceProvider, mapper, configuration)
        {
        }

        public async Task<ExchangeDocumentDTO?> CreateAsync(ExchangeDocumentDTO exchangeDto)
        {
            try
            {
                var document = Mapper.Map<ExchangeDocument>(exchangeDto);
                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(ExchangeDocument), JsonSerializer.Serialize(document));
                var ret = await Connection.InsertAsync<ExchangeDocument>(document);

                return await GetAsync(exchangeDto.Id);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to create {TableName} record: {Message}", nameof(ExchangeDocument), e.Message);
                return null;
            }

        }

        public async Task<ExchangeDocumentDTO?> GetAsync(string exchangeId)
        {
            try
            {
                Logger.LogDebug("Fetching {TableName} for {ItemId}", nameof(ExchangeDocument), exchangeId);
                var exchange = await Connection.GetAsync<ExchangeDocument>(exchangeId);
                return Mapper.Map<ExchangeDocumentDTO>(exchange);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to fetch {TableName} record: {Message}", nameof(ExchangeDocument), e.Message);
                return null;
            }
        }

    }
}
