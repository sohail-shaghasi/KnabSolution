using Knab.Exchange.Core.Configurations;
using Knab.Exchange.Exchangerates.ApiClient.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;

namespace Knab.Exchange.UnitTest.ExchangeRatesUnitTest
{
    /// <summary>
    /// Naming Convention
    /// The name of the test consists of three parts: 
    /// 1. The name of the method being tested. 
    /// 2. The scenario under which it's being tested. 
    /// 3. The expected behavior when the scenario is invoked.
    /// </summary>

    [TestClass]
    public class ExchangeRatesServiceUnitTest
    {
        private readonly string _exchangeApiUrl = "http://api.exchangeratesapi.io";
        private readonly string _AccessKey = "X-CMC_PRO_API_KEY";
        private readonly string _apiKeyValue = "394c82c13b6f20a72b68a23a177aa257";

        private const decimal BTCVALUE = 40000.1M;
        
        
        [TestMethod]
        [DataRow("BTC", "USD")]
        public void GetExchangeRatesList_invokeApiEndpoint_ShouldReturnResult(string baseCurrency, string targetCurrencies)
        {
            //Arrange

            var listOfTargetCurrencies = new List<string>();
            listOfTargetCurrencies.Add(targetCurrencies);
            var httpClientStub = new Mock<IHttpClientFactory>();
            var logger = Mock.Of<ILogger<ExchangeRatesService>>();
          
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
                BaseAddress = new Uri(_exchangeApiUrl),
            };
            httpClientStub.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            //mockconfiguration
            var serviceConfigurations = new ServiceConfigurations();
            serviceConfigurations.ExchangeratesApi = new ExchangeratesApi()
            {
               ServiceBaseUrl = _exchangeApiUrl,
               AccessKey = _AccessKey,
               ExchangeRateEndpoint = "/latest",
               TargetedCurrencies = listOfTargetCurrencies,
               Version = "V1"
            };

            // We need to set the Value of IOptions to be the ServiceConfigurations Class
            var configStub = new Mock<IOptions<ServiceConfigurations>>();
            configStub.Setup(ap => ap.Value).Returns(serviceConfigurations);
            ExchangeRatesService exchangeRatesService = new ExchangeRatesService(logger, configStub.Object, httpClientStub.Object);

            //Act
            var result =  exchangeRatesService.GetExchangeRatesList(baseCurrency, listOfTargetCurrencies).Result;
            
            //Assert
            Assert.AreEqual(baseCurrency, result.BaseCurrencySymbol);
        }
    }
}
