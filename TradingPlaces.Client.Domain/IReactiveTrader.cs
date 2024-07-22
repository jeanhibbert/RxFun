using System;
using TradingPlaces.Client.Domain.Instrumentation;
using TradingPlaces.Client.Domain.Repositories;
using TradingPlaces.Shared.Logging;

namespace TradingPlaces.Client.Domain
{
    public interface IReactiveTrader
    {
        IReferenceDataRepository ReferenceData { get; }
        ITradeRepository TradeRepository { get; }
        IObservable<ConnectionInfo> ConnectionStatusStream { get; }
        IPriceLatencyRecorder PriceLatencyRecorder { get; }
        IControlRepository Control { get; }
        void Initialize(string username, string[] servers, ILoggerFactory loggerFactory = null, string authToken = null);
    }
}