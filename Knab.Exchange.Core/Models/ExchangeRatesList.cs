
namespace Knab.Exchange.Core.Models
{
    public class ExchangeRatesList
    {
        /// <summary>
        /// This is a common Model class used for consolidating the api results and preparing it for the 
        /// output
        /// </summary>
        public ExchangeRatesList()
        {
            CurrenciesRates = new Dictionary<string, decimal>();
        }
        public string BaseCurrencySymbol { set; get; }
        public Dictionary<string,decimal> CurrenciesRates { set; get; }
    }
}
