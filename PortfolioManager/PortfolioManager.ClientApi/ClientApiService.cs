using System.Net.Http;

namespace PortfolioManager.ClientApi
{
    public class ClientApiService
    {
        public HttpClient HttpClient { get; private set; }
        public ClientApiService(IHttpClientFactory factory)
        {
            HttpClient = factory.Get();
        }
    }
}
