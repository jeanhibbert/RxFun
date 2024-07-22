using System;
using System.Collections.Generic;
using TradingPlaces.Shared.DTO;
using TradingPlaces.Shared.DTO.Control;

namespace TradingPlaces.Client.Domain.ServiceClients
{
    public interface IControlServiceClient
    {
        IObservable<UnitDto> SetPriceFeedThroughput(FeedThroughputDto request);
        IObservable<IEnumerable<CurrencyPairStateDto>> GetCurrencyPairStates();
        IObservable<UnitDto> SetCurrencyPairState(CurrencyPairStateDto request);
        IObservable<FeedThroughputDto> GetPriceFeedThroughput();
    }
}