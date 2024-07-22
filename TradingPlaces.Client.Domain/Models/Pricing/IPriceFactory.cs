using TradingPlaces.Client.Domain.Models.ReferenceData;
using TradingPlaces.Shared.DTO.Pricing;

namespace TradingPlaces.Client.Domain.Models.Pricing
{
    internal interface IPriceFactory
    {
        IPrice Create(PriceDto price, ICurrencyPair currencyPair);
    }
}