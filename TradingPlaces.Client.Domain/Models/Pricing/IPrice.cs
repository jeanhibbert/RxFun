using System;
using TradingPlaces.Client.Domain.Models.ReferenceData;

namespace TradingPlaces.Client.Domain.Models.Pricing
{
    public interface IPrice
    {
        IExecutablePrice Bid { get; }
        IExecutablePrice Ask { get; }
        decimal Mid { get; }
        ICurrencyPair CurrencyPair { get; }
        DateTime ValueDate { get; }
        decimal Spread { get; }
        bool IsStale { get; }
    }
}
