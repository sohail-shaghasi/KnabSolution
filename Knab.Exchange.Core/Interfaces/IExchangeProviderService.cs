using Knab.Exchange.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knab.Exchange.Core.Interfaces
{
    public interface IExchangeProviderService
    {
        Task<ExchangeRatesList> GetExchangeRatesList(string BaseCurrencySymbol);
    }
}
