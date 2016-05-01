namespace Trades.Refactored.TradeTests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Trades.Refactored.TradeLib;

    public class TestTrades
    {
        private Trade _myTrade;

        private TradeExecutor _myExecutor;

        [SetUp]
        public void SetUp()
        {
            this._myTrade = new Trade();
            this._myTrade.SecurityName = "IBM";

            var tickerSet = new HashSet<string> { "IBM", "GOOG" };

            var tickerSource = new TickerSample(tickerSet);
            this._myExecutor = new TradeExecutor(tickerSource);
        }

        [Test]
        public void CannotExcuteWithZeroQty() // Test #1
        {
            this._myTrade.Quantity = 0;
            this._myExecutor.ExecuteTrade(this._myTrade);
            Assert.That(this._myTrade.IsExcuted, Is.False);
        }

        [Test]
        public void CanExcuteWithNormalLotQty()  // Test #2
        {
            this._myTrade.Quantity = 100;
            this._myExecutor.ExecuteTrade(this._myTrade);
            Assert.That(this._myTrade.IsExcuted, Is.True);
        }

        [Test]
        public void CannotExcuteArrayOfOddsQty()  // Test #3
        {
            var expected = new bool[99];
            var values = new bool[99];

            for (int q = 1; q < 100; q++)
            {
                this._myTrade.Quantity = q;
                values[q - 1] = this._myExecutor.ExecuteTrade(this._myTrade);
            }

            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void CanExcuteVeryLargeQty()  // Test #4
        {
            this._myTrade.Quantity = 50000000; // up to 50M
            this._myExecutor.ExecuteTrade(this._myTrade);
            Assert.That(this._myTrade.IsExcuted, Is.True);
        }

        [Test]
        public void CannotExcuteOverTheLimitQty()  // Test #5
        {
            this._myTrade.Quantity = 50000100; // over 50M
            this._myExecutor.ExecuteTrade(this._myTrade);
            Assert.That(this._myTrade.IsExcuted, Is.False);
        }

        [Test]
        public void CanExcuteRandomSetOfValidQuantities()  // Test #6
        {
            var expected = new bool[5];
            var values = new bool[5];
            var rand = new Random();

            int ii = 0;
            while (true)
            {
                var qty = rand.Next(100, 50000000);
                if (qty % 100 == 0)
                {
                    expected[ii] = true;
                    this._myTrade.Quantity = qty;
                    values[ii] = this._myExecutor.ExecuteTrade(this._myTrade);
                    ii++;
                    if (ii >= 5) break;
                }
            }

            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void CannotExcuteRandomSetOfInvalidQuantities()  // Test #16
        {
            var expected = new bool[5];
            var values = new bool[5];
            var rand = new Random();

            int ii = 0;
            while (true)
            {
                var qty = rand.Next(100, 50000000);
                if (qty % 100 != 0)
                {
                    this._myTrade.Quantity = qty;
                    values[ii] = this._myExecutor.ExecuteTrade(this._myTrade);
                    ii++;
                    if (ii >= 5) break;
                }
            }

            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void CannotExcuteNegativeQty()  // Test #17
        {
            this._myTrade.Quantity = -100;
            this._myExecutor.ExecuteTrade(this._myTrade);
            Assert.That(this._myTrade.IsExcuted, Is.False);
        }

        [Test]
        public void CannotExcuteNegativeAndOddQty()  // Test #18
        {
            this._myTrade.Quantity = -101;
            this._myExecutor.ExecuteTrade(this._myTrade);
            Assert.That(this._myTrade.IsExcuted, Is.False);
        }

        [Test]
        public void CannotExecuteWithNoSecurityName()  // Test #19
        {
            var trade = new Trade();
            trade.SecurityName = string.Empty;
            trade.Quantity = 100;
            this._myExecutor.ExecuteTrade(trade);
            Assert.That(trade.IsExcuted, Is.False);
        }

        [Test]
        public void CannotExcuteInvalidTicker()  // Test #20
        {
            var trade = new Trade();
            trade.SecurityName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            trade.Quantity = 100;
            this._myExecutor.ExecuteTrade(trade);
            Assert.That(trade.IsExcuted, Is.False);
        }

        #region additional_tests

        [Test]
        public void CannotExcuteOddQty()
        {
            this._myTrade.Quantity = 101;
            this._myExecutor.ExecuteTrade(this._myTrade);
            Assert.That(this._myTrade.IsExcuted, Is.False);
        }

        [Test]
        public void CannotExcuteNullTicker()
        {
            var trade = new Trade();
            trade.SecurityName = null;
            trade.Quantity = 100;
            this._myExecutor.ExecuteTrade(trade);
            Assert.That(trade.IsExcuted, Is.False);
        }

        [Test]
        public void CannotExcuteUnknownTicker()
        {
            var trade = new Trade();
            trade.SecurityName = "????";
            trade.Quantity = 100;
            this._myExecutor.ExecuteTrade(trade);
            Assert.That(trade.IsExcuted, Is.False);
        }

        #endregion
    }
}
