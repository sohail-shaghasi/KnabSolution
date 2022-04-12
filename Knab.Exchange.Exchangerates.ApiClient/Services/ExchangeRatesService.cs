using Knab.Exchange.Core.Interfaces;
using Knab.Exchange.Core.Models;
using Knab.Exchange.Exchangerates.ApiClient.CustomException;
using Knab.Exchange.Exchangerates.ApiClient.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using O.WP.CMC.UmmAdapterService.Core.Configurations;
using System.Web;

namespace Knab.Exchange.Exchangerates.ApiClient.Services
{
    public class ExchangeRatesService : IExchangeApiClientService
    {

        protected readonly ServiceConfigurations _appConfigs;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ExchangeRatesService> _logger;

        public string Name { get { return "ExchangeRatesService"; } }

        public ExchangeRatesService(ILogger<ExchangeRatesService> logger, IOptions<ServiceConfigurations> appConfigs, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _appConfigs = appConfigs.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ExchangeRatesList> GetExchangeRatesList(string baseCurrencySymbol, List<string> targetCurrencies)
        {

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var query = HttpUtility.ParseQueryString(String.Empty);
                //base parameter is not accepted in the free plan
                query["base"] = baseCurrencySymbol;
                query["symbols"] = string.Join(",", targetCurrencies.Select(e => e.ToUpper()).ToArray());
                query["access_key"] = _appConfigs.ExchangeratesApi.AccessKey;
                var request = new HttpRequestMessage(HttpMethod.Get, $"/{_appConfigs.ExchangeratesApi.Version}/{_appConfigs.ExchangeratesApi.ExchangeRateEndpoint}?{query}");
                var httpclient = _httpClientFactory.CreateClient("ExchangeRatesApi");
                response = await httpclient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var exchangeRateApiResponse = JsonConvert.DeserializeObject<ExchangeRateApiResponse>(responseString);
                if (response.IsSuccessStatusCode)
                {

                    return new ExchangeRatesList()
                    {
                        BaseCurrencySymbol = exchangeRateApiResponse.BaseCurrency,
                        CurrenciesRates = exchangeRateApiResponse.Rates
                    };
                }
            }
            catch (ExchangeRatesHttpException ex)
            {
                //throw will have a stack trace with it.
                _logger.LogError(String.Format("ExchangeRatesApi Status Code returned {0}", response.StatusCode));
                throw;
            }
            return new ExchangeRatesList();
        }
    }




}
