using Knab.Exchange.Core.Interfaces;
using Knab.Exchange.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knab.Exchange.Core.Services
{
    public class ExchangeProviderService : IExchangeProviderService
    {
        public Task<ExchangeRatesList> GetExchangeRatesList(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
