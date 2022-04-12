using Knab.Exchange.CoinMarketCap.ApiClient.Services;
using Knab.Exchange.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using O.WP.CMC.UmmAdapterService.Core.Configurations;
using Polly;
using Polly.Extensions.Http;

namespace Knab.Exchange.CoinMarketCap.ApiClient.Injections
{
    public static class CoinMarketCapInjections
    {
        public static void InjectCointMarketCapApiService(this IServiceCollection services, ServiceConfigurations serviceConfigurations)
        {
            // Register CointMarketcap's dependencies
            services.AddHttpClient("CoinmarketcapApi", httpclient =>
              {
                  httpclient.BaseAddress = new Uri(serviceConfigurations.CoinmarketcapApi.BaseUrl);
                  httpclient.DefaultRequestHeaders.Add("Accept", "application/json");
              })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Reuse the handlers within 5 minutes lifetime
                .AddPolicyHandler(GetRetryPolicy());

            services.AddSingleton<IExchangeApiClientService, CoinMarketCapService>();

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
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
