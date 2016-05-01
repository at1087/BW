namespace Trades.Refactored.TradeLib
{
    using System.Collections.Generic;

    public interface ITickerSource
    {
        HashSet<string> GetTickerSet();
    }
}
