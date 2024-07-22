using System;
using System.Reactive.Linq;
using TradingPlaces.Client.Domain.Transport;
using TradingPlaces.Shared;
using TradingPlaces.Shared.DTO.Execution;
using Microsoft.AspNet.SignalR.Client;

namespace TradingPlaces.Client.Domain.ServiceClients
{
    internal class ExecutionServiceClient : ServiceClientBase, IExecutionServiceClient
    {
        public ExecutionServiceClient(IConnectionProvider connectionProvider) : base(connectionProvider)
        {
        }

        public IObservable<TradeDto> ExecuteRequest(TradeRequestDto tradeRequest)
        {
            return RequestUponConnection(connection => ExecuteForConnection(connection.ExecutionHubProxy, tradeRequest), TimeSpan.FromMilliseconds(500));
        }

        private static IObservable<TradeDto> ExecuteForConnection(IHubProxy executionHubProxy, TradeRequestDto tradeRequestDto)
        {
            return Observable.FromAsync(
                () => executionHubProxy.Invoke<TradeDto>(ServiceConstants.Server.Execute, tradeRequestDto));
        } 
    }
}
