
namespace Knab.Exchange.Core.Interfaces
{
    public interface IExchangeProviderService
    {
        Task<ExchangeRatesList> GetExchangeRatesAsync(string BaseCryptocurrencySymbol);
    }
}
