using Serilog;
using Serilog.Events;

namespace O.WP.CMC.UmmAdapterService.Core.Configurations
{
    public class ServiceConfigurations
    {
        public string Environment { get; set; }
        public ServiceInfo ServiceInfo { get; set; }
        public CoinmarketcapApi CoinmarketcapApi { get; set; }
        public Exchangeratesapi Exchangeratesapi { get; set; }
        public LoggingSinks LoggingSinks { get; set; }
        public Logging Logging { get; set; }
    }

    public class ServiceInfo
    {
        public string Name { get; set; }
        public string RepositoryUrl { get; set; }
    }

    public class CoinmarketcapApi
    {
        public string BaseUrl { get; set; }
        public string Version { get; set; }
        public string QuotesEndpoint { get; set; }
        public string MapEndpoint { get; set; }
        public string APIKeyName { get; set; }
        public string APIKeyValue { get; set; }
        public List<string> DefaultTargetedCurrencies { get; set; }
        public List<string> SupportedTargetedCurrencies { get; set; }
    }

    public class Exchangeratesapi
    {
        public string ServiceBaseUrl { get; set; }
        public string ExchangeRateEndpoint { get; set; }
        public List<string> SupportedCurrencies { get; set; }
        public List<string> DefaultTargetedCurrencies { get; set; }
        public bool EnableCaching { get; set; }
        public int ExpiredAfterInMinutes { get; set; }
    }

    public class Console
    {
        public bool Enabled { get; set; }
        public string OutputTemplate { get; set; }
        public LogEventLevel LogLevel { get; set; }
    }

    public class RollingFile
    {
        public bool Enabled { get; set; }
        public string Location { get; set; }
        public string Extension { get; set; }
        public string OutputTemplate { get; set; }
        public RollingInterval RollingInterval { get; set; }
        public LogEventLevel MimimumLevel { get; set; }
    }

    public class LoggingSinks
    {
        public Console Console { get; set; }
        public RollingFile RollingFile { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
        public LogEventLevel MimimumLevel { get; set; }
        
    }

}
