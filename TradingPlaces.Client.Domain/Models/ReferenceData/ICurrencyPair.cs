using System;
using TradingPlaces.Client.Domain.Models.Pricing;

namespace TradingPlaces.Client.Domain.Models.ReferenceData
{
    public interface ICurrencyPair
    {
        string Symbol { get; }
        IObservable<IPrice> PriceStream { get; }
        int RatePrecision { get; }
        int PipsPosition { get; }
        string BaseCurrency { get; }
        string CounterCurrency { get; }
    }
}
