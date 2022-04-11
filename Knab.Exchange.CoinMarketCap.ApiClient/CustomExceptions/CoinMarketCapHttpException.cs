using System.Net;

namespace Knab.Exchange.CoinMarketCap.ApiClient.CustomException
{
    [Serializable()]
    public class CoinMarketCapHttpException : HttpRequestException
    {
        public CoinMarketCapHttpException(string ServiceName, HttpStatusCode httpStatusCode, string ResponseAsString) : base(string.Format("Request to [{0}] failed with following response: {1}", ServiceName, ResponseAsString))
        { }

        public CoinMarketCapHttpException()
        {
        }

        public CoinMarketCapHttpException(string message) : base(message)
        {
        }

        public CoinMarketCapHttpException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
