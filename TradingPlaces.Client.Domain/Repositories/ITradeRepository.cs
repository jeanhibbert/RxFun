using System;
using System.Collections.Generic;
using TradingPlaces.Client.Domain.Models.Execution;

namespace TradingPlaces.Client.Domain.Repositories
{
    public interface ITradeRepository
    {
        IObservable<IEnumerable<ITrade>> GetTradesStream();
    }
}