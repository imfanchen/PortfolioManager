using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace PortfolioManager.ClientApi
{
    /// <summary>
    /// HttpClient should be reused across application lift time
    /// Do not dispose this object
    /// </summary>
    public interface IHttpClientFactory
    {
        HttpClient Get();
    }
}
