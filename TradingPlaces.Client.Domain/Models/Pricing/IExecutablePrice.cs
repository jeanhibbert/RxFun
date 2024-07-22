using System;
using TradingPlaces.Client.Domain.Models.Execution;
using TradingPlaces.Shared.Extensions;

namespace TradingPlaces.Client.Domain.Models.Pricing
{
    public interface IExecutablePrice
    {
        IObservable<IStale<ITrade>> ExecuteRequest(long notional, string dealtCurrency);
        Direction Direction { get; }
        IPrice Parent { get; }
        decimal Rate { get; }
    }
}
