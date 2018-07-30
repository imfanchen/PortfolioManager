using System;
using System.Collections.Generic;

namespace PortfolioManager.Common
{
    public class Security
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsEnabled { get; set; }
        public string Type { get; set; }
        public int IEXId { get; set; }

        public static Dictionary<string, string> TypeDictionary = new Dictionary<string, string>
        {
            {"AD", "American Depositary Receipt"},
            {"RE","Real Estate Investment Trust"},
            {"CE", "Close End Fund"},
            {"SI", "Secondary Issue"},
            {"CS", "Common Stock"},
            {"PS", "Prefered Stock"},
            {"ET", "Prefered Stock"},
            {"ETF", "Exchange Traded Fund"},
            {"CRYPTO", "Crypto Currency"},
        };
    }
}
