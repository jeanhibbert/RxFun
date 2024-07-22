using TradingPlaces.Shared.DTO.ReferenceData;

namespace TradingPlaces.Client.Domain.Models.ReferenceData
{
    internal interface ICurrencyPairUpdateFactory
    {
        ICurrencyPairUpdate Create(CurrencyPairUpdateDto currencyPairUpdate);
    }
}