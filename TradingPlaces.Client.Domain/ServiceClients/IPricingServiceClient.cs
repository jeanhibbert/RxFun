using System;
using TradingPlaces.Shared.DTO.Pricing;

namespace TradingPlaces.Client.Domain.ServiceClients
{
    internal interface IPricingServiceClient
    {
        IObservable<PriceDto> GetSpotStream(string currencyPair);
    }
}