using Knab.Exchange.CoinMarketCap.ApiClient.CustomException;
using Knab.Exchange.CoinMarketCap.ApiClient.Model;
using Knab.Exchange.Core.Interfaces;
using Knab.Exchange.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using O.WP.CMC.UmmAdapterService.Core.Configurations;
using System.Web;

namespace Knab.Exchange.CoinMarketCap.ApiClient.Services
{
    public class CoinMarketCapService : IExchangeProviderService
    {
        protected Dictionary<string, int> supportedCryptoCurrencies;
        protected readonly ServiceConfigurations _appConfigs;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CoinMarketCapService> _logger;

        public CoinMarketCapService(ILogger<CoinMarketCapService> logger, IOptions<ServiceConfigurations> appConfigs, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _appConfigs = appConfigs.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ExchangeRatesList> GetExchangeRatesList(string BaseCurrencySymbol)//, params string[] TargetedCurrencies)
        {
            BaseCurrencySymbol = BaseCurrencySymbol.ToUpper();
            var targetedCurrencies = _appConfigs.CoinmarketcapApi.TargetCurrencies.Select(e => e.ToUpper()).ToArray();
          
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var query = HttpUtility.ParseQueryString(String.Empty);
                query["symbol"] = BaseCurrencySymbol;
                query["convert"] = string.Join(",", targetedCurrencies);
                query["aux"] = "is_active"; //customize your request. and limit the response
                var request = new HttpRequestMessage(HttpMethod.Get, $"/{_appConfigs.CoinmarketcapApi.Version}/{_appConfigs.CoinmarketcapApi.QuotesEndpoint}?{query}");
                request.Headers.Add(_appConfigs.CoinmarketcapApi.APIKeyName, _appConfigs.CoinmarketcapApi.APIKeyValue);
                var httpclient = _httpClientFactory.CreateClient("CoinmarketcapApi");
                    response = await httpclient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var CoinmarketcapResponse = JsonConvert.DeserializeObject<CoinmarketcapAPIQuotesResponse>(responseString);
                if (response.IsSuccessStatusCode)
                {
                    if (CoinmarketcapResponse?.Status?.ErrorCode == 0
                        && CoinmarketcapResponse.Status.ErrorMessage == null
                        && CoinmarketcapResponse.Status.CreditCount > 0
                        && CoinmarketcapResponse.Data?.Count > 0
                        && CoinmarketcapResponse.Data.Count > 0
                        && CoinmarketcapResponse.Data.ContainsKey(BaseCurrencySymbol))
                    {
                        return new ExchangeRatesList()
                        {
                            BaseCurrencySymbol = CoinmarketcapResponse.Data[BaseCurrencySymbol][0].Symbol,
                            CurrenciesRates = CoinmarketcapResponse.Data[BaseCurrencySymbol][0].Quote.Where(e => targetedCurrencies.Contains(e.Key)).ToDictionary(key => key.Key, val => val.Value.Price)
                        };
                    }

                }
            }
            catch (CoinMarketCapHttpException ex)
            {
                _logger.LogError(String.Format("CoinMarketCap Status Code returned {0}", response.StatusCode));
                //throw will have a stack trace with it.
                throw; 
            }
            return new ExchangeRatesList();
        }
    }
}
