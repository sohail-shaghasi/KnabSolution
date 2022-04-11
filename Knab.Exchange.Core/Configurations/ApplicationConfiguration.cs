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
        public string Version { get; set; }
        public string Description { get; set; }
        public string Maintainer { get; set; }

    }

    public class CoinmarketcapApi
    {
        public bool Enabled { get; set; }
        public string BaseUrl { get; set; }
        public string Version { get; set; }
        public string QuotesEndpoint { get; set; }
        public string APIKeyName { get; set; }
        public string APIKeyValue { get; set; }
        public List<string> TargetCurrencies { get; set; }
    }

    public class Exchangeratesapi
    {
        public bool Enabled { get; set; }
        public string ServiceBaseUrl { get; set; }
        public string ExchangeRateEndpoint { get; set; }
        public List<string> DefaultTargetedCurrencies { get; set; }
        public string Version { get; set; }
        public string AccessKey { get; set; }
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
