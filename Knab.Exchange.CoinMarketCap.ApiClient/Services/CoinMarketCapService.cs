namespace Knab.Exchange.CoinMarketCap.ApiClient.Services
{
    public class CoinMarketCapService : IExchangeApiClientService
    {
        protected readonly ServiceConfigurations _appConfigs;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CoinMarketCapService> _logger;

        public string Name { get { return "CoinMarketCapService"; } }

        public CoinMarketCapService(ILogger<CoinMarketCapService> logger, IOptions<ServiceConfigurations> appConfigs, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _appConfigs = appConfigs.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ExchangeRatesList> GetExchangeRatesList(string BaseCurrencySymbol, List<string> targetCurrencies)
        {
            BaseCurrencySymbol = BaseCurrencySymbol.ToUpper();
            var currencies = targetCurrencies.Select(e => e.ToUpper()).ToArray();
          
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var query = HttpUtility.ParseQueryString(String.Empty);
                query["symbol"] = BaseCurrencySymbol;
                query["convert"] = string.Join(",", currencies);
                query["aux"] = "is_active"; //customize your request. and limit the response
                var request = new HttpRequestMessage(HttpMethod.Get, $"/{_appConfigs.CoinmarketcapApi.Version}/{_appConfigs.CoinmarketcapApi.QuotesEndpoint}?{query}");
                request.Headers.Add(_appConfigs.CoinmarketcapApi.APIKeyName, _appConfigs.CoinmarketcapApi.APIKeyValue);
                var httpclient = _httpClientFactory.CreateClient("CoinmarketcapApi");
                    response = await httpclient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var coinmarketcapResponse = JsonConvert.DeserializeObject<CoinmarketcapAPIQuotesResponse>(responseString);
                if (response.IsSuccessStatusCode)
                {
                    if (coinmarketcapResponse?.Status?.ErrorCode == 0
                        && coinmarketcapResponse.Status.ErrorMessage == null
                        && coinmarketcapResponse.Status.CreditCount > 0
                        && coinmarketcapResponse.Data?.Count > 0
                        && coinmarketcapResponse.Data.Count > 0
                        && coinmarketcapResponse.Data.ContainsKey(BaseCurrencySymbol))
                    {
                        return new ExchangeRatesList()
                        {
                            BaseCurrencySymbol = coinmarketcapResponse.Data[BaseCurrencySymbol][0].Symbol,
                            CurrenciesRates = coinmarketcapResponse.Data[BaseCurrencySymbol][0].Quote.Where(e => currencies.Contains(e.Key)).ToDictionary(key => key.Key, val => val.Value.Price)
                        };
                    }

                }
                return new ExchangeRatesList();
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
