
namespace Knab.Exchange.CoinMarketCap.ApiClient.Model
{
    public class CoinmarketcapAPIQuotesResponse
    {
        [JsonProperty("status")]
        public CoinmarketcapResponseStatus Status { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, List<CoinmarketcapResponseQuotes>> Data { set; get; }
    }
   
    public class CoinmarketcapResponseQuotes
    {
        [JsonProperty("id")]
        public int CryptoCurrencyId { set; get; }

        [JsonProperty("name")]
        public string CryptoCurrencyName { set; get; }

        [JsonProperty("symbol")]
        public string Symbol { set; get; }

        [JsonProperty("slug")]
        public string Slug { set; get; }

        [JsonProperty("is_active")]
        public int IsActive { set; get; }

        [JsonProperty("quote")]
        public Dictionary<string, CoinmarketcapResponseQuote> Quote { set; get; }

    }

    public class CoinmarketcapResponseQuote
    {
        [JsonProperty("price")]
        public decimal Price { set; get; }

    }
    public class CoinmarketcapResponseStatus
    {
        [JsonProperty("error_code")]
        public int ErrorCode { set; get; }
        
        [JsonProperty("error_message")]
        public string ErrorMessage { set; get; }
        
        [JsonProperty("credit_count")]
        public int CreditCount { set; get; }
        
        [JsonProperty("notice")]
        public string Notice { set; get; }

    }


}
