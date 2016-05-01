namespace Trades.Original.TradeLib
{
    public class TradeExecutor
    {
        public bool ExecuteTrade(Trade trade_)
        {
            // original code
            // if odd lots failed.
            if (trade_.Quantity % 100 != 0) return false;

            // trade is good, update trade
            trade_.IsExcuted = true;

            return true;
        }
    }
}
