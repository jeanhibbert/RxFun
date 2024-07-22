using System;
using System.Collections.Generic;
using TradingPlaces.Shared.DTO.Execution;

namespace TradingPlaces.Client.Domain.ServiceClients
{
    internal interface IBlotterServiceClient
    {
        IObservable<IEnumerable<TradeDto>> GetTradesStream();
    }
}