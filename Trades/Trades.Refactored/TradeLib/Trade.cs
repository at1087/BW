namespace Trades.Refactored.TradeLib
{
    public class Trade
    {
        public string SecurityName { get; set; }
        public int Quantity { get; set; }
        public bool IsExcuted { get; set; }
    }
}
