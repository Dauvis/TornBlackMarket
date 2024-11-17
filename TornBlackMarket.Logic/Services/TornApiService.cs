using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System.Text.Json;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Common.Util;

namespace TornBlackMarket.Logic.Services
{
    public class TornApiService : ITornApiService
    {
        const string _userEndpoint = "user";
        const string _basicSelection = "basic";
        const string _bazaarSelection = "bazaar";

        private readonly string _v2BaseUri;

        private readonly HttpClient _httpClient;
        private readonly ILogger<TornApiService> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public TornApiService(IConfiguration configuration, HttpClient httpClient, ILogger<TornApiService> logger)
        {
            _v2BaseUri = configuration["Torn:V2Base"] ?? throw new ArgumentException("Torn:V2Base not found in appsettings.json", nameof(configuration));
            _httpClient = httpClient;
            _logger = logger;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private string GetV2ApiUrl(string apiKey, string endpoint, List<string> selections, string optSelectionId = "")
        {
            // https://api.torn.com/v2/user?key=xxxxxxxxxxxxxxxx&selections=basic,bazaar
            string optSelectionIdPart = string.IsNullOrEmpty(optSelectionId) ? "" : "&id=" + optSelectionId;
            return $"{_v2BaseUri}/{endpoint}?key={apiKey}&selections={string.Join(",", selections)}{optSelectionIdPart}";
        }

        public async Task<TornPlayerDTO?> GetUserBasicAsync(string apiKey)
        {
            var url = GetV2ApiUrl(apiKey, _userEndpoint, [_basicSelection]);
            return await CallTornApiAsync<TornPlayerDTO>(url);
        }

        private async Task<T?> CallTornApiAsync<T>(string url) where T : class
        {
            _logger.LogDebug("Calling Torn API: {URL}", url);
            using HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Torn API call failed ({Code}): {Headers}", response.StatusCode, response.Headers.ToString());

                var apiFailErrorDto = new TornErrorDTO()
                {
                    Error = new()
                    {
                        Code = 1025,
                        Message = $"API call failed {response.StatusCode}"
                    }
                };

                return (T?)GenericsUtil.CallStaticMethod<T>("CreateError", _logger, apiFailErrorDto); ;
            }

            using HttpContent httpContent = response.Content;
            var content = await httpContent.ReadAsStringAsync();

            var errorDto = JsonSerializer.Deserialize<TornErrorDTO>(content, _jsonSerializerOptions);

            if (errorDto is not null && errorDto.Error.Code != 0)
            {
                _logger.LogDebug("API call returned error {Code}: {Message}", errorDto.Error.Code, errorDto.Error.Message);
                return (T?)GenericsUtil.CallStaticMethod<T>("CreateError", _logger, errorDto);
            }

            var contentDto = JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
            return contentDto;
        }
    }
}
