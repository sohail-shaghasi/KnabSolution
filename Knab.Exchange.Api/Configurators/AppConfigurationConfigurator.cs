namespace Knab.Exchange.Api.Configurators
{
    /// <summary>
    /// Extensions methods to setup the app configurations in the service collection.
    /// Works for both <see cref="IHostBuilder"/> and <see cref="IWebHostBuilder"/>.
    /// </summary>
    internal static class AppConfigurationConfigurator
    {
        /// <summary>
        /// Extension method for setting up app configuration for an instance of <see cref="IHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureAppConfiguration(this IHostBuilder builder) =>
            builder.ConfigureAppConfiguration(ConfigureAppConfiguration);

        /// <summary>
        /// Sets up app configuration for the <see cref="HostBuilderContext" />.
        /// </summary>
        /// <param name="hostBuilderContext"></param>
        /// <param name="configurationBuilder"></param>
        private static void ConfigureAppConfiguration(HostBuilderContext hostBuilderContext,
            IConfigurationBuilder configurationBuilder) =>
            ConfigureAppConfiguration(configurationBuilder);

        /// <summary>
        /// Extension method for setting up app configuration for an instance of <see cref="IWebHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigureAppConfiguration(this IWebHostBuilder builder) =>
            builder.ConfigureAppConfiguration(ConfigureAppConfiguration);

        /// <summary>
        /// Sets up app configuration for the <see cref="WebHostBuilderContext" />.
        /// </summary>
        /// <param name="webHostBuilderContext"></param>
        /// <param name="configurationBuilder"></param>
        private static void ConfigureAppConfiguration(WebHostBuilderContext webHostBuilderContext,
            IConfigurationBuilder configurationBuilder) =>
            ConfigureAppConfiguration(configurationBuilder);

        /// <summary>
        /// Sets up app configuration for the <see cref="IConfigurationBuilder" />.
        /// </summary>
        /// <param name="configurationBuilder"></param>
        private static void ConfigureAppConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .AddEnvironmentVariables()
                .AddJsonFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"),
                    optional: false);

            var runtimeEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.Equals(runtimeEnvironment, "LocalDevelopment"))
            {
                configurationBuilder.AddJsonFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"appsettings.{runtimeEnvironment}.json"),
                    optional: false);
            }
        }
    }

}
