using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager.Common;

namespace PortfolioManager.ClientApi
{
    public class ClientApiException : Exception
    {
        public ErrorMessage Error { get; private set; }
        public ClientApiException(ErrorMessage error) : base(error.ExceptionMessage)
        {
            Error = error;
        }
    }
}
