
namespace Knab.Exchange.Api.UnitTest
{
    [TestClass]
    public class CurrencyExchangeControllerTest
    {
        private const decimal btcValue = 40000.1M;

        /// <summary>
        /// Naming Convention
        /// The name of the test consists of three parts: 
        /// 1. The name of the method being tested. 
        /// 2. The scenario under which it's being tested. 
        /// 3. The expected behavior when the scenario is invoked.
        /// </summary>
        [TestMethod]
        public void CurrencyExchangeController_InvokeConstructor_ShouldReturnInstance()
        {
            CurrencyExchangeController testObject = new (null);
            Assert.IsNotNull(testObject);
        }

        [TestMethod]
        [DataRow("BTC", "USD")]
        public async Task GetCurrencyQuotesAsync_currencyQoutes_ShouldReturnOkResult(string baseCurrency, string targetCurrency)
        {
            // Arrange
            Dictionary<string, decimal> expectedCurrenciesRates = new() {{ targetCurrency, btcValue } };
            var stub = new Mock<IExchangeProviderService>(); 
            
            stub.Setup(x => x.GetExchangeRatesAsync(It.IsAny<string>())).Returns(Task.FromResult(
                        new ExchangeRatesList
                        {  BaseCurrencySymbol = baseCurrency,
                           CurrenciesRates = expectedCurrenciesRates
                        }));


            
            CurrencyExchangeController currencyExchangeControllerObject = new (stub.Object);

            //Act
            var actualCurrenciesRates = await currencyExchangeControllerObject.GetCurrencyQuotesAsync(baseCurrency);

            //Assert
            Assert.IsInstanceOfType(actualCurrenciesRates, typeof(OkObjectResult));
        }

        [TestMethod]
        [DataRow("BTC", "USD")]
        public async Task GetCurrencyQuotesAsync_currencyQoutes_ShouldReturnNotFoundResult(string baseCurrency, string targetCurrency)
        {
            // Arrange
            var expectedCurrenciesRates = new Dictionary<string, decimal>() { { targetCurrency, btcValue } };
            var stub = new Mock<IExchangeProviderService>();

            stub.Setup(x => x.GetExchangeRatesAsync(It.IsAny<string>())).Returns(Task.FromResult(
                        new ExchangeRatesList
                        {
                            BaseCurrencySymbol = baseCurrency,
                            CurrenciesRates = expectedCurrenciesRates
                        }));

            CurrencyExchangeController currencyExchangeControllerObject = new CurrencyExchangeController(stub.Object);

            //Act
            var actualCurrenciesRates = await currencyExchangeControllerObject.GetCurrencyQuotesAsync(baseCurrency);

            //Assert
            Assert.IsInstanceOfType(actualCurrenciesRates, typeof(OkObjectResult));
        }
    }
}
