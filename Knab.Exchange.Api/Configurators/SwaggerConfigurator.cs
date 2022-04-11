using O.WP.CMC.UmmAdapterService.Core.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Knab.Exchange.Api.Configurators
{
    internal static class SwaggerConfigurator
    {
        #region IServiceCollection

        public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection,
            WebHostBuilderContext webHostBuilderContext)
        {
            var applicationConfiguration = webHostBuilderContext
                .Configuration
                .GetSection("ServiceConfigurations")
                .Get<ServiceConfigurations>();

            serviceCollection.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions
                    .AddSwaggerDoc(applicationConfiguration);
            });

            return serviceCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerGenOptions"></param>
        /// <param name="serviceConfigurations"></param>
        private static SwaggerGenOptions AddSwaggerDoc(this SwaggerGenOptions swaggerGenOptions,
            ServiceConfigurations serviceConfigurations)
        {
            var name = $"{serviceConfigurations.ServiceInfo.Name}-{serviceConfigurations.ServiceInfo.Version}";

            swaggerGenOptions
                .SwaggerDoc(name,
                    new()
                    {
                        Title = serviceConfigurations.ServiceInfo.Name,
                        Description = $"{serviceConfigurations.ServiceInfo.Description}" +
                            $"<br/>Maintainer: {serviceConfigurations.ServiceInfo.Maintainer}" +
                            $"<br/>Git Repository: <a href='{serviceConfigurations.ServiceInfo.RepositoryUrl}' target='_blank'>{serviceConfigurations.ServiceInfo.RepositoryUrl}</a>" +
                            $"<br/>Version: {serviceConfigurations.ServiceInfo.Version}"
                    });

            return swaggerGenOptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="isDevelopment"></param>
        /// <param name="serviceConfigurations"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerGen(this IApplicationBuilder applicationBuilder,
            bool isDevelopment,
            ServiceConfigurations serviceConfigurations)
        {

            applicationBuilder.UseSwagger(serviceConfigurations);

            return applicationBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="serviceConfigurations"></param>
        /// <returns></returns>
        private static IApplicationBuilder UseSwagger(this IApplicationBuilder applicationBuilder,
            ServiceConfigurations serviceConfigurations)
        {
            applicationBuilder
                .UseDeveloperExceptionPage()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"/swagger/{$"{serviceConfigurations.ServiceInfo.Name}-{serviceConfigurations.ServiceInfo.Version}"}/swagger.json",
                        $"{serviceConfigurations.ServiceInfo.Name} (v{serviceConfigurations.ServiceInfo.Version})");
                });

            return applicationBuilder;
        }

        #endregion
    }
}
