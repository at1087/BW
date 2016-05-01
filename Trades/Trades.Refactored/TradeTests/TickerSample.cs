namespace Trades.Refactored.TradeTests
{
    using System.Collections.Generic;

    using Trades.Refactored.TradeLib;

    class TickerSample : ITickerSource
    {
        private readonly HashSet<string> _tickerSet;

        public TickerSample(HashSet<string> tickerSet_)
        {
            this._tickerSet = tickerSet_;
        }
        public HashSet<string> GetTickerSet()
        {
            return this._tickerSet;
        }
    }
}
