using Newtonsoft.Json;

namespace Knab.Exchange.Exchangerates.ApiClient.Models
{
    public class ExchangeRateApiResponse
    {
        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { set; get; }
        [JsonProperty("base")]
        public string BaseCurrency { set; get; }
    }
}
