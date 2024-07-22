using System;
using System.Collections.Generic;
using TradingPlaces.Client.Domain.Models.ReferenceData;

namespace TradingPlaces.Client.Domain.Repositories
{
    public interface IReferenceDataRepository
    {
        IObservable<IEnumerable<ICurrencyPairUpdate>> GetCurrencyPairsStream();
    }
}
