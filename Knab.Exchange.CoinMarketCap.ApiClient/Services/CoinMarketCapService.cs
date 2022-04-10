using App.Components.CoinmarketcapApiClient.Model;
using Knab.Exchange.Core.Interfaces;
using Knab.Exchange.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using O.WP.CMC.UmmAdapterService.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            var targetedCurrencies = _appConfigs.CoinmarketcapApi.SupportedTargetedCurrencies.Select(e => e.ToUpper()).ToArray();
            if (!supportedCryptoCurrencies.ContainsKey(BaseCurrencySymbol))
            {
                //TODO: add custom exception
                throw new Exception();
            }
            if (targetedCurrencies == null || targetedCurrencies.Length == 0)
            {
                targetedCurrencies = _appConfigs.CoinmarketcapApi.DefaultTargetedCurrencies.ToArray();
            }
            else
            {
                var unsupportedCurrencies = targetedCurrencies.Except(_appConfigs.CoinmarketcapApi.SupportedTargetedCurrencies);
               
                if (unsupportedCurrencies.Any())
                {
                    _logger.LogWarning("[{0}] invalid fiat currencies", string.Join(",", unsupportedCurrencies));
                    targetedCurrencies = targetedCurrencies.Except(unsupportedCurrencies).ToArray();
                }
            }
            string cryptoCurId = supportedCryptoCurrencies[BaseCurrencySymbol].ToString();
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["id"] = cryptoCurId;
            query["convert"] = string.Join(",", targetedCurrencies);
            query["aux"] = "is_active";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/{_appConfigs.CoinmarketcapApi.Version}/{_appConfigs.CoinmarketcapApi.QuotesEndpoint}?{query}");
            request.Headers.Add(_appConfigs.CoinmarketcapApi.APIKeyName, _appConfigs.CoinmarketcapApi.APIKeyValue);
            var httpclient = _httpClientFactory.CreateClient("CoinmarketcapApi");
            var response = await httpclient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var CoinmarketcapAPIResponse = JsonConvert.DeserializeObject<CoinmarketcapAPIQuotesResponse>(responseString);
            if (response.IsSuccessStatusCode)
            {
                if (CoinmarketcapAPIResponse?.Status?.ErrorCode == 0
                    && CoinmarketcapAPIResponse.Status.ErrorMessage == null
                    && CoinmarketcapAPIResponse.Status.CreditCount > 0
                    && CoinmarketcapAPIResponse.Data?.Count > 0
                    && CoinmarketcapAPIResponse.Data.Count > 0
                    && CoinmarketcapAPIResponse.Data.ContainsKey(cryptoCurId)
                    )
                {
                    return new ExchangeRatesList()
                    {
                        BaseCurrencySymbol = CoinmarketcapAPIResponse.Data[cryptoCurId].CryptoCurrencySymbol,
                        CurrenciesRates = CoinmarketcapAPIResponse.Data[cryptoCurId].Quote.Where(e => targetedCurrencies.Contains(e.Key)).ToDictionary(key => key.Key, val => val.Value.Rate)
                    };
                }

            }
            //TODO: throw custom exception
            throw new Exception(String.Format("CoinMarketCap Status Code returned {0} , errorMessage{1}", response.StatusCode, CoinmarketcapAPIResponse.Status.ErrorMessage));
        }
    }
}
