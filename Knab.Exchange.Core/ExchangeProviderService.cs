
namespace Knab.Exchange.Core
{
    public class ExchangeProviderService : IExchangeProviderService
    {
        private readonly ServiceConfigurations _serviceConfigurations;
        private readonly IEnumerable<IExchangeApiClientService> _exchangeApiClientService;
        public ExchangeProviderService(IOptions<ServiceConfigurations> serviceConfigurations, IEnumerable<IExchangeApiClientService> exchangeApiClientService)
        {
            _serviceConfigurations = serviceConfigurations.Value;
            _exchangeApiClientService = exchangeApiClientService;
        }
        public async Task<ExchangeRatesList> GetExchangeRatesAsync(string BaseCryptocurrencySymbol)
        {
            return await GetCollectiveRatesAsync(BaseCryptocurrencySymbol);
        }

        /// <summary>
        /// This functions returns collective result from both Exchanges(CoinmarketCap, ExchangeRates) 
        /// since free plan for both Apis returning limited currencies therefore merging the result from both 
        /// to fulfil the acceptance criteria.
        /// </summary>
        /// <param name="BaseCryptocurrencySymbol"></param>
        /// <returns></returns>

        private async Task<ExchangeRatesList> GetCollectiveRatesAsync(string BaseCryptocurrencySymbol)
        {
            var fiatBasedCurrency = _serviceConfigurations.CoinmarketcapApi.TargetCurrencies.FirstOrDefault();
            var cointMarketCapApiClient = GetApiClientServiceByName(_serviceConfigurations.CoinmarketcapApi.ExchangeName);// _exchangeApiClientService.FirstOrDefault(e => e.Name == _serviceConfigurations.CoinmarketcapApi.ExchangeName);
            var firstListOfRates = cointMarketCapApiClient.GetExchangeRatesList(BaseCryptocurrencySymbol, _serviceConfigurations.CoinmarketcapApi.TargetCurrencies);
            var ExchangeRatesApiClient = GetApiClientServiceByName(_serviceConfigurations.ExchangeratesApi.ExchangeName);
            var secondListOfRates = ExchangeRatesApiClient.GetExchangeRatesList(fiatBasedCurrency, _serviceConfigurations.ExchangeratesApi.TargetedCurrencies);

            var results = await Task.WhenAll(firstListOfRates, secondListOfRates);

            var baseCurrencySymbol = results[0].BaseCurrencySymbol;
            var cryptoValue = results[0].CurrenciesRates.FirstOrDefault(e => e.Key.Equals(fiatBasedCurrency)).Value;

            return new ExchangeRatesList()
            {
                BaseCurrencySymbol = results[0].BaseCurrencySymbol,
                CurrenciesRates = results[1].CurrenciesRates.Select(e => new { e.Key, value =  e.Value * cryptoValue }).ToDictionary(key=>key.Key , v=>v.value)
            };
        }

        public IExchangeApiClientService GetApiClientServiceByName(string apiServiceName)
        {
            return _exchangeApiClientService.FirstOrDefault(e => e.Name == apiServiceName);
        }
    }
}

