

namespace Knab.Exchange.Core.Interfaces
{
    public interface IExchangeApiClientService
    {
        Task<ExchangeRatesList> GetExchangeRatesList(string baseCurrencySymbol, List<string> targetCurrencies);
        string Name { get;}
    }
}
