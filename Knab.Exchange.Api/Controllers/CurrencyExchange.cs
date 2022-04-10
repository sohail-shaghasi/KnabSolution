using Knab.Exchange.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knab.Exchange.Api.Controllers
{
    [Route("api/CurrencyExchange")]
    [ApiController]
    public class CurrencyExchange : ControllerBase
    {
        private readonly IExchangeProviderService _exchangeProvider;

        public CurrencyExchange(IExchangeProviderService exchangeProvider)
        {
            _exchangeProvider = exchangeProvider;
        }

        [HttpGet("{symbol}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult> QuotesAsync(string symbol)
        {
            var results = await _exchangeProvider.GetExchangeRatesList(symbol);
            return Ok(results);
        }
    }
}
