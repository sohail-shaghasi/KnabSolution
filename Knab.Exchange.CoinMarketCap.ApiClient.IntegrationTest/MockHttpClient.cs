namespace Knab.Exchange.CoinMarketCap.ApiClient.IntegrationTest
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}
