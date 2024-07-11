using System;

namespace TradingPlaces.Server.Domain.Model
{
    public interface IPrice
    {
        string Ticker { get; }
        decimal Price { get; }
        DateTime ValueDateTime { get; }
        ITickerData TickerData { get; }
    }

    public class PriceDto : IPrice
    {
        public PriceDto (string ticker, ITickerData tickerData, decimal price)
        {
            Ticker = ticker;
            TickerData = tickerData;
            Price = price;
            ValueDateTime = DateTime.Now;
        }

        public decimal Price { get; private set; }

        public DateTime ValueDateTime { get; private set; }

        public string Ticker { get; }

        public ITickerData TickerData { get; private set; }

        public override string ToString()
        {
            return $"Ticker {Ticker} - PRICE: [{Price}] : {ValueDateTime.ToLongTimeString()}";
        }
    }
}
