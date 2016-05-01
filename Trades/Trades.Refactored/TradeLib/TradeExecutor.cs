namespace Trades.Refactored.TradeLib
{
    using System.Collections.Generic;

    public class TradeExecutor
    {
        private readonly HashSet<string> _validTickers;

        public TradeExecutor(ITickerSource tickerSource_)
        {
            this._validTickers = tickerSource_.GetTickerSet();
        }

        private bool IsValidSecurityName(string securityName_)
        {
            if (!this._validTickers.Contains(securityName_)) return false;
            return true;
        }
        public bool ExecuteTrade(Trade trade_)
        {
            // refactor for null, zero or negative quantyty and invalid ticker
            if (trade_ == null) return false;

            if (string.IsNullOrEmpty(trade_.SecurityName)) return false;

            if (trade_.Quantity <= 0) return false;

            // factor for over the limit
            if (trade_.Quantity > 50000000) return false;

            // refactor for adding collection of valid tickers
            if (!this.IsValidSecurityName(trade_.SecurityName)) return false;

            // original code
            // if odd lots failed.
            if (trade_.Quantity % 100 != 0) return false;

            // trade is good, update trade
            trade_.IsExcuted = true;

            return true;
        }
    }
}
