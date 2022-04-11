using Microsoft.Extensions.Logging;
using O.WP.CMC.UmmAdapterService.Core.Configurations;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace Knab.Exchange.Api.Configurators
{
    internal static class LoggingConfigurator
    {
        private static readonly object _syncLock = new();
        private static Logger _logger;



        /// <summary>
        /// Extension method for setting up logging configuration for an instance of <see cref="IWebHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder builder) =>
            builder.ConfigureLogging(ConfigureLogging);

        /// <summary>
        /// Sets up logging configuration for the <see cref="WebHostBuilderContext" />.
        /// </summary>
        /// <param name="builderContext"></param>
        /// <param name="loggingBuilder"></param>
        private static void ConfigureLogging(WebHostBuilderContext builderContext,
            ILoggingBuilder loggingBuilder)
        {
            var serviceConfiguration = builderContext
                .Configuration
                .GetSection("ServiceConfigurations")
                .Get<ServiceConfigurations>();

            ConfigureLogging(loggingBuilder, serviceConfiguration);
        }

        /// <summary>
        /// Sets up logging configuration for the <see cref="ILoggingBuilder" />.
        /// </summary>
        /// <param name="loggingBuilder"></param>
        /// <param name="serviceConfiguration"></param>
        private static void ConfigureLogging(ILoggingBuilder loggingBuilder,
            ServiceConfigurations serviceConfiguration)
        {
            // This is to ensure that both the Host and WebHost are using the same logger configuration

            if (_logger == null)
            {
                lock (_syncLock)
                {
                    var loggerConfiguration = CreateLoggerConfiguration(serviceConfiguration);

                    ConfigureConsole(loggerConfiguration, serviceConfiguration);
                    ConfigureRollingFile(loggerConfiguration, serviceConfiguration);

                    _logger = loggerConfiguration.CreateLogger();
                }
            }

            loggingBuilder.AddSerilog(_logger, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationConfiguration"></param>
        /// <returns></returns>
        private static LoggerConfiguration CreateLoggerConfiguration(ServiceConfigurations serviceConfigurations)
        {
            var levelSwitch = new LoggingLevelSwitch()
            {
                MinimumLevel = serviceConfigurations.Logging.MimimumLevel
            };

            return new LoggerConfiguration()
               .MinimumLevel.ControlledBy(levelSwitch)
               .MinimumLevel.Override("Microsoft", levelSwitch.MinimumLevel)
               .Enrich.FromLogContext()
               .Enrich.WithProperty("AppName", serviceConfigurations.ServiceInfo.Name)
               .Enrich.WithProperty("Environment", serviceConfigurations.Environment)
               .Enrich.WithMachineName()
               .Enrich.WithEnvironmentUserName()
               .Destructure.ToMaximumDepth(10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="applicationConfiguration"></param>
        private static void ConfigureConsole(LoggerConfiguration loggerConfiguration,
            ServiceConfigurations serviceConfigurations)
        {
            if (serviceConfigurations.LoggingSinks.Console.Enabled)
            {
                loggerConfiguration
                    .WriteTo
                    .Console(outputTemplate: serviceConfigurations.LoggingSinks.Console.OutputTemplate,
                        restrictedToMinimumLevel: serviceConfigurations.LoggingSinks.Console.LogLevel,
                        theme: SystemConsoleTheme.Literate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="applicationConfiguration"></param>
        private static void ConfigureRollingFile(LoggerConfiguration loggerConfiguration,
            ServiceConfigurations serviceConfigurations)
        {
            if (serviceConfigurations.LoggingSinks.RollingFile.Enabled)
            {
                EnsureDirectory(serviceConfigurations.LoggingSinks.RollingFile.Location);

                var fileName = $"{serviceConfigurations.ServiceInfo.Name}.{serviceConfigurations.LoggingSinks.RollingFile.Extension}";
                var filePath = Path.Combine(serviceConfigurations.LoggingSinks.RollingFile.Location, fileName);
                loggerConfiguration
                    .WriteTo
                    .File(filePath,
                        rollingInterval: serviceConfigurations.LoggingSinks.RollingFile.RollingInterval,
                        restrictedToMinimumLevel: serviceConfigurations.LoggingSinks.RollingFile.MimimumLevel,
                        outputTemplate: serviceConfigurations.LoggingSinks.RollingFile.OutputTemplate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sinkFolder"></param>
        private static void EnsureDirectory(string sinkFolder)
        {
            if (!Directory.Exists(sinkFolder))
            {
                Directory.CreateDirectory(sinkFolder);
            }
        }
    }
}
