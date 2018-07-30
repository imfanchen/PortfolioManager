using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager.Common;

namespace PortfolioManager.ClientApi
{
    public class SecuritiesClientApi : ClientApiService
    {
        public SecuritiesClientApi(IHttpClientFactory factory) : base(factory) { }

        public async Task<IEnumerable<Security>> GetSymbols()
        {
            string url = new StringBuilder().Reference("Symbols").ToString();
            var response = await HttpClient.GetAsync(url);
            return await response.Handle<IEnumerable<Security>>();
        }

        public async Task<Company> GetCompany(string symbol)
        {
            string url = new StringBuilder().Find(symbol, nameof(Company)).ToString().ToLower();
            var response = await HttpClient.GetAsync(url);
            return await response.Handle<Company>();
        }

        public async Task<Quote> GetQuote(string symbol)
        {
            string url = new StringBuilder().Find(symbol, nameof(Quote)).ToString().ToLower();
            var response = await HttpClient.GetAsync(url);
            return await response.Handle<Quote>();
        }

        public async Task<IEnumerable<Chart>> GetChart(string symbol)
        {
            string url = new StringBuilder().Find(symbol, nameof(Chart)).ToString().ToLower();
            var response = await HttpClient.GetAsync(url);
            return await response.Handle<IEnumerable<Chart>>();
        }       
    }
}
