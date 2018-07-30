using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager.Common;

namespace PortfolioManager.ClientApi
{
    public static class ClientApiExtension
    {
        public static StringBuilder Reference(this StringBuilder sb, string data)
        {
            return sb.Append($"ref-data/{data}");
        }

        public static StringBuilder Find(this StringBuilder sb, string symbol, string data)
        {
            return sb.Append($"stock/{symbol}/{data}");
        }

        public static StringBuilder Action(this StringBuilder sb, string controler, string name)
        {
            return sb.Append(controler).Append("/").Append(name).Append("?");
        }

        public static StringBuilder BuildParam(this StringBuilder sb, string name, object value)
        {
            sb.Append(name).Append("=");
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    value = Convert.ToInt32(value);                
                }
                sb.Append(Uri.EscapeDataString(Convert.ToString(value)));
            }
            sb.Append("&");
            return sb;
        }

        public static StringBuilder BuildArrayParam(this StringBuilder sb, string name, IEnumerable values)
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    sb.BuildParam(name, value);
                }
            }
            else
            {
                sb.BuildParam(name, null);
            }
            return sb;
        }

        public static async Task<T> Handle<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            else
            {
                ErrorMessage msg = await response.Content.ReadAsAsync<ErrorMessage>();
                throw new ClientApiException(msg);
            }
        }
    }
}
