using Knab.Exchange.CoinMarketCap.ApiClient.CustomException;
using Knab.Exchange.Core.Interfaces;
using Knab.Exchange.Core.Models;
using Knab.Exchange.Exchangerates.ApiClient.CustomException;
using Microsoft.AspNetCore.Mvc;

namespace Knab.Exchange.Api.Controllers
{
    [ApiController]
    [Route("api/CurrencyExchange")]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly IExchangeProviderService _exchangeProvider;

        public CurrencyExchangeController(IExchangeProviderService exchangeProvider)
        {
            _exchangeProvider = exchangeProvider;
        }

        [HttpGet("{symbol}")]
        [ProducesResponseType(typeof(ExchangeRatesList), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult> GetCurrencyQuotesAsync(string symbol)
        {
            try
            {
                var exchangeRatesResult = await _exchangeProvider.GetExchangeRatesAsync(symbol);
                if (exchangeRatesResult != null)
                {
                    return this.Ok(exchangeRatesResult);
                }
                return this.NotFound();
            }
            catch (CoinMarketCapHttpException coinMarketCapHttpException)
            {
                return Problem($"Internal server error, there is a problem interacting with CoinMarketCap Api. {coinMarketCapHttpException.Message}");
            }
            catch (ExchangeRatesHttpException exchangeRatesHttpException)
            {
                return Problem($"Internal server error, there is a problem interacting with ExchangeRates Api. {exchangeRatesHttpException.Message}");
            }
            catch
            {
                return Problem($"Unable to get fiat currency Quotes for base currency:  {symbol}.");
            }
        }
    }
}
