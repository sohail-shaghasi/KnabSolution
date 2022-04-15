

namespace Knab.Exchange.Exchangerates.ApiClient.CustomException
{
    public class ExchangeRatesHttpException : HttpRequestException
    {
        public ExchangeRatesHttpException(string ServiceName, HttpStatusCode httpStatusCode, string ResponseAsString) : base(string.Format("Request to [{0}] failed with following response: {1}", ServiceName, ResponseAsString))
        { }

        public ExchangeRatesHttpException()
        {
        }

        public ExchangeRatesHttpException(string message) : base(message)
        {
        }

        public ExchangeRatesHttpException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
