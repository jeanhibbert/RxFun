using System;

namespace TradingPlaces.Client.Domain.Transport
{
    internal interface IConnectionProvider
    {
        IObservable<IConnection> GetActiveConnection();
    }
}