using Knab.Exchange.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knab.Exchange.Core.Interfaces
{
    public interface IExchangeApiClientService
    {
        Task<ExchangeRatesList> GetExchangeRatesList(string baseCurrencySymbol, List<string> targetCurrencies);
        string Name { get;}
    }
}
