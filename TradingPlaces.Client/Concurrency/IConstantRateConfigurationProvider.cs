using System;

namespace TradingPlaces.Client.Concurrency
{
    public interface IConstantRateConfigurationProvider
    {
        TimeSpan ConstantRate { get; }
    }
}