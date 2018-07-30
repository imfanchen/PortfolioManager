using System;
using System.Configuration;
using System.Net.Http;

namespace PortfolioManager.ClientApi
{
    public class HttpClientFactory : IHttpClientFactory
    {
        static Lazy<HttpClient> client = new Lazy<HttpClient>(() =>
        {
            var handler = new HttpClientHandler() { UseDefaultCredentials = true };
            string url = ConfigurationManager.AppSettings[WebApiBaseUrl];
            if (string.IsNullOrEmpty(url))
            {
                throw new ConfigurationErrorsException($"Missing AppSetting with Key: {WebApiBaseUrl}");
            }
            return new HttpClient(handler)
            {
                BaseAddress = new Uri(url),
                Timeout = TimeSpan.FromMinutes(15)
            };
        });
        public const string WebApiBaseUrl = "WebApiBaseUrl";
        public HttpClient Get()
        {
            return client.Value;
        }
    }
}
