using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager.Common
{
    public class Quote
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string PrimaryExchange { get; set; }
        public string Sector { get; set; }
        public string CalculationPrice { get; set; }
        public decimal? Open { get; set; }
        public decimal? OpenTime { get; set; }
        public decimal? Close { get; set; }
        public decimal? CloseTime { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? LatestPrice { get; set; }
        public string LatestSource { get; set; }
        public string LatestTime { get; set; }
        public decimal? LatestUpdate { get; set; }
        public decimal? LatestVolume { get; set; }
        public decimal? DelayedPrice { get; set; }
        public decimal? DelayedPriceTime { get; set; }
        public decimal? ExtendedPrice { get; set; }
        public decimal? ExtendedPriceTime { get; set; }
        public decimal? ExtendedChange { get; set; }
        public decimal? ExtendedChangePercent { get; set; }
        public decimal? PreviousClose { get; set; }
        public decimal? Change { get; set; }
        public decimal? ChangePercent { get; set; }
        public decimal? AvgTotalVolume { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? PERatio { get; set; }
        public decimal? Week52High { get; set; }
        public decimal? Week52Low { get; set; }
        public decimal? YTDChange { get; set; }
        public decimal? IEXRealtimePrice { get; set; }
        public decimal? IEXRealtimeSize { get; set; }
        public decimal? IEXLastUpdated { get; set; }
        public decimal? IEXMarketPercent { get; set; }
        public decimal? IEXVolume { get; set; }
        public decimal? IEXBidPrice { get; set; }
        public decimal? IEXBidSize { get; set; }
        public decimal? IEXAskPrice { get; set; }
        public decimal? IEXAskSize { get; set; }

    }
}
