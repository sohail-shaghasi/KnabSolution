using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Knab.Exchange.CoinMarketCap.ApiClient.Injections
{
    public static class CoinMarketCapInjections
    {
        public static void InjectDependencies(this IServiceCollection services, WebHostBuilderContext hostContext)
        {
            // Register CointMarketcap's dependencies
            _ = services.AddHttpClient("CoinmarketcapApi", c =>
              {
                  string serviceurl = hostContext.Configuration["CoinmarketcapAPI:baseUrl"];
                  c.BaseAddress = new Uri(serviceurl);
                  c.DefaultRequestHeaders.Add("Accept", "application/json");
              }).SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Reuse the handlers within 5 minutes lifetime
                 .AddPolicyHandler(GetRetryPolicy());

        }
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            //Attempt 1: 2seconds
            //Attempt 2: 4seconds
            //Attempt 3: 8seconds

            //TODO: trigger the retry mechanism based on the custom exception thrown.
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)));
        }
    }
}
