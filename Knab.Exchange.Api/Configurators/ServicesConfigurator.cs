using Knab.Exchange.CoinMarketCap.ApiClient.Injections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using O.WP.CMC.UmmAdapterService.Core.Configurations;


namespace Knab.Exchange.Api.Configurators
{
    /// <summary>
    /// Extension and helper methods for configuring the services for the host.
    /// The extension method <see cref="IServiceCOllection.InjectDependencies" /> is called for your
    /// specific services to be injected.
    /// </summary>
    internal static class ServicesConfigurator
    {
        /// <summary>
        /// Extension method for configuring the services in the <see cref="IHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureServices(this IHostBuilder builder) =>
            builder.ConfigureServices(ConfigureServices);

        /// <summary>
        /// Helper method for adding services to the the <see cref="HostBuilderContext" />.
        /// </summary>
        /// <param name="hostBuilderContext"></param>
        /// <param name="serviceCollection"></param>
        private static void ConfigureServices(HostBuilderContext hostBuilderContext,
            IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();

            var section = hostBuilderContext
                .Configuration
                    .GetSection("ServiceConfigurations");

            serviceCollection.Configure<ServiceConfigurations>(section);
        }


        /// <summary>
        /// Extension method for configuring the services in the <see cref="IWebHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigureServices(this IWebHostBuilder builder) =>
            builder.ConfigureServices(ConfigureServices);

        /// <summary>
        /// Helper method for adding services to the the <see cref="WebHostBuilderContext" />.
        /// </summary>
        /// <param name="hostBuilderContext"></param>
        /// <param name="serviceCollection"></param>
        private static void ConfigureServices(WebHostBuilderContext hostBuilderContext,
            IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();

            var serviceConfigurations = hostBuilderContext.Configuration.GetSection("ServiceConfigurations");

            serviceCollection.Configure<ServiceConfigurations>(serviceConfigurations);

            // NOTE: In order to separate dependency injection concerns, each api Client will have its own extension method.
            // This will help a new 3rd party exchange's api is introduced and ready to consume.
            CoinMarketCapInjections.InjectDependencies(serviceCollection, hostBuilderContext);
            ExchangeRatesInjections.InjectDependencies(serviceCollection, hostBuilderContext);
        }
    }

}
