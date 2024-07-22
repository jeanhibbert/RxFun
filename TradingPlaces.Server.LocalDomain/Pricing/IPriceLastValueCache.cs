using TradingPlaces.Shared.DTO.Pricing;

namespace TradingPlaces.Server.Pricing
{
    public interface IPriceLastValueCache
    {
        PriceDto GetLastValue(string currencyPair);
        void StoreLastValue(PriceDto price);
    }
}