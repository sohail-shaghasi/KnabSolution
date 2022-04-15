
namespace Knab.Exchange.Api.Builders
{
    /// <summary>
    /// A default provided class that is being used for building the WebApplication.
    /// </summary>
    public static class DefaultBuilder
    {
        private static ServiceConfigurations _serviceConfigurations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static async Task<WebApplication> ExecuteAsync(this WebApplicationBuilder builder,
            CancellationToken cancellationToken = default)
        {
            var webHost = builder.WebHost;

            // Configurations
            webHost
                .ConfigureAppConfiguration()
                .ConfigureServices()
                .ConfigureLogging();

            // Configuration File
            _serviceConfigurations = builder
                .Configuration
                .GetSection("ServiceConfigurations")
                .Get<ServiceConfigurations>();

            // App initiations
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Mappings
            app.MapControllers();

            // Swagger
            app.UseSwaggerGen(builder.Environment.IsDevelopment(),_serviceConfigurations);

            // Run
            await app
                .RunAsync(cancellationToken);
            return app;
        }
    }
}
