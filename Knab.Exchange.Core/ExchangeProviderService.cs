using Knab.Exchange.Core.Interfaces;
using Knab.Exchange.Core.Models;
using Microsoft.Extensions.Options;
using O.WP.CMC.UmmAdapterService.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var cointMarketCapApiClient = GetApiCleint(_serviceConfigurations.CoinmarketcapApi.ExchangeName);// _exchangeApiClientService.FirstOrDefault(e => e.Name == _serviceConfigurations.CoinmarketcapApi.ExchangeName);
            var firstListOfRates = cointMarketCapApiClient.GetExchangeRatesList(BaseCryptocurrencySymbol, _serviceConfigurations.CoinmarketcapApi.TargetCurrencies);
            var ExchangeRatesApiClient = GetApiCleint(_serviceConfigurations.ExchangeratesApi.ExchangeName);
            var secondListOfRates = ExchangeRatesApiClient.GetExchangeRatesList(_serviceConfigurations.CoinmarketcapApi.TargetCurrencies.FirstOrDefault(), _serviceConfigurations.ExchangeratesApi.TargetedCurrencies);

            var results = await Task.WhenAll(firstListOfRates, secondListOfRates);

            return new ExchangeRatesList()
            {
                BaseCurrencySymbol = results[0].BaseCurrencySymbol,
                CurrenciesRates = results[1].CurrenciesRates
            };
        }

        public IExchangeApiClientService GetApiCleint(string apiServiceName)
        {
            return _exchangeApiClientService.FirstOrDefault(e => e.Name == apiServiceName);
        }
    }
}

