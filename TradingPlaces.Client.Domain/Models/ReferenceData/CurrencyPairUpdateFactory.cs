using TradingPlaces.Client.Domain.Repositories;
using TradingPlaces.Shared.DTO;
using TradingPlaces.Shared.DTO.ReferenceData;

namespace TradingPlaces.Client.Domain.Models.ReferenceData
{
    internal class CurrencyPairUpdateFactory : ICurrencyPairUpdateFactory
    {
        private readonly IPriceRepository _priceRepository;

        public CurrencyPairUpdateFactory(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public ICurrencyPairUpdate Create(CurrencyPairUpdateDto currencyPairUpdate)
        {
            var cp = new CurrencyPair(
                currencyPairUpdate.CurrencyPair.Symbol,
                currencyPairUpdate.CurrencyPair.RatePrecision,
                currencyPairUpdate.CurrencyPair.PipsPosition,
                _priceRepository);

            var update =
                new CurrencyPairUpdate(
                    currencyPairUpdate.UpdateType == UpdateTypeDto.Added ? UpdateType.Add : UpdateType.Remove, cp);

            return update;
        }
    }
}