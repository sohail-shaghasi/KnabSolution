using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Knab.Exchange.CoinMarketCap.ApiClient.IntegrationTest
{
    /// <summary>
    /// Naming Convention
    /// The name of the test consists of three parts: 
    /// 1. The name of the method being tested. 
    /// 2. The scenario under which it's being tested. 
    /// 3. The expected behavior when the scenario is invoked.
    /// </summary>
    [TestClass]
    public class CoinMarketCapServiceIntegrationTest
    {
        private readonly string _coinmarketcapApiUrl = "https://pro-api.coinmarketcap.com";
        private readonly string _apiKeyName = "X-CMC_PRO_API_KEY";
        private readonly string _apiKeyValue = "77756464-e009-4f26-9d95-966f54b65338";
        private readonly string _apiVersion = "v2";
        //private readonly ILogger<CoinMarketCapService> _logger = new Mock<ILogger<CoinMarketCapService>>().Object;

        [TestMethod]
        [DataRow("BTC", "USD")]
        public async Task GetExchangeRatesList_currencyExchangeRates_ShouldReturnOkResult(string baseCurrency, string targetCurrency)
        {
            //Arrange
            //var config = InitConfiguration();

            //var section = config.GetSection("ServiceConfigurations");
            //var serviceConfigurations = section.Get<ServiceConfigurations>();


            //using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            //    .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace)
            //    .AddConsole());

            //ILogger<CoinMarketCapService> logger = loggerFactory.CreateLogger<CoinMarketCapService>();

            //List<string> currencies = new List<string>();
            //currencies.Add(targetCurrency);
            
            //CoinMarketCapService cointMarketCapService = new CoinMarketCapService(logger, (IOptions<ServiceConfigurations>)serviceConfigurations, new MockHttpClientFactory());
            //var result = cointMarketCapService.GetExchangeRatesList(baseCurrency, currencies);
        }

        public static IConfiguration InitConfiguration()
        {
           var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();
            return config;
        }



    }
}
