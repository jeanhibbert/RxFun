using System;

namespace TradingPlaces.Shared.Logging
{
    public interface ILoggerFactory
    {
        ILog Create(Type type);
    }
}