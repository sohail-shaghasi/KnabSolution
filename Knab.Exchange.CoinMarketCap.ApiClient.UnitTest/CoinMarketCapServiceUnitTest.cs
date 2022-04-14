using Knab.Exchange.CoinMarketCap.ApiClient.Services;
using Knab.Exchange.Core.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Knab.Exchange.CoinMarketCap.ApiClient.UnitTest
{
    /// <summary>
    /// Naming Convention
    /// The name of the test consists of three parts: 
    /// 1. The name of the method being tested. 
    /// 2. The scenario under which it's being tested. 
    /// 3. The expected behavior when the scenario is invoked.
    /// </summary>
    [TestClass]
    public class CoinMarketCapServiceUnitTest
    {
        private readonly string _coinMarketCapApiUrl = "https://pro-api.coinmarketcap.com";
        private readonly string _quotesEndpoint = "cryptocurrency/quotes/latest";
        private readonly string _apiKey = "X-CMC_PRO_API_KEY";
        private readonly string _apikeyName = "X-CMC_PRO_API_KEY";
        private readonly string _apiKeyValue = "77756464-e009-4f26-9d95-966f54b65338";
        private readonly string _apiversion = "2";

        private const decimal BTCVALUE = 40000.1M;

      

        [TestMethod]
        [DataRow("BTC", "EUR")]
        public void GetExchangeRatesList_invokeApiEndpoint_ShouldReturnResult(string baseCurrency, string targetCurrencies)
        {
            //Arrange

            var listOfTargetCurrencies = new List<string>();
            listOfTargetCurrencies.Add(targetCurrencies);
            var httpClientStub = new Mock<IHttpClientFactory>();
            var logger = Mock.Of<ILogger<CoinMarketCapService>>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            string jsonResponse = @" { 'base': '" + baseCurrency + "' } ";
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // use real http client with mocked handler here
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(_coinMarketCapApiUrl),
            };
            httpClientStub.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            //mockconfiguration
            var serviceConfigurations = new ServiceConfigurations();
            serviceConfigurations.CoinmarketcapApi = new CoinmarketcapApi()
            {
               APIKeyName = _apiKey,
               APIKeyValue = _apiKeyValue,
               BaseUrl = _coinMarketCapApiUrl,
               QuotesEndpoint = _quotesEndpoint,
               TargetCurrencies = listOfTargetCurrencies,
               Version = _apiversion
            };

            // We need to set the Value of IOptions to be the ServiceConfigurations Class
            var configStub = new Mock<IOptions<ServiceConfigurations>>();
            configStub.Setup(ap => ap.Value).Returns(serviceConfigurations);
            CoinMarketCapService exchangeRatesService = new CoinMarketCapService(logger, configStub.Object, httpClientStub.Object);

            //Act
            var result = exchangeRatesService.GetExchangeRatesList(baseCurrency, listOfTargetCurrencies).Result;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
