
namespace Knab.Exchange.CoinMarketCap.ApiClient.Injections
{
    public static class ExchangeRatesInjections
    {
        public static void InjectExchangeRatesApiService(this IServiceCollection services, ServiceConfigurations serviceConfigurations)
        {
            // Register CointMarketcap's dependencies
            services.AddHttpClient("ExchangeRatesApi", httpclient =>
            {
                httpclient.BaseAddress = new Uri(serviceConfigurations.ExchangeratesApi.ServiceBaseUrl);
                httpclient.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Reuse the handlers within 5 minutes lifetime
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddSingleton<IExchangeApiClientService, ExchangeRatesService>();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            //Attempt 1: 2seconds
            //Attempt 2: 4seconds
            //Attempt 3: 8seconds

            //retry when other than success result is returned.
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            // Break the request circuit after 5 retries for 30 seconds 
            return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
