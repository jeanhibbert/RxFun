using System;
using TradingPlaces.Shared.DTO.Execution;

namespace TradingPlaces.Client.Domain.ServiceClients
{
    internal interface IExecutionServiceClient
    {
        IObservable<TradeDto> ExecuteRequest(TradeRequestDto tradeRequest);
    }
}