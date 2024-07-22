using System;
using TradingPlaces.Client.Domain.Models.Pricing;
using TradingPlaces.Client.Domain.Models.ReferenceData;

namespace TradingPlaces.Client.Domain.Repositories
{
    interface IPriceRepository
    {
        IObservable<IPrice> GetPriceStream(ICurrencyPair currencyPair);
    }
}