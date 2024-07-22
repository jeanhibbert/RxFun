using System;
using TradingPlaces.Client.Domain.Models.Execution;
using TradingPlaces.Client.Domain.Models.Pricing;
using TradingPlaces.Shared.Extensions;

namespace TradingPlaces.Client.Domain.Repositories
{
    interface IExecutionRepository
    {
        IObservable<IStale<ITrade>> ExecuteRequest(IExecutablePrice executablePrice, long notional, string dealtCurrency);
    }
}