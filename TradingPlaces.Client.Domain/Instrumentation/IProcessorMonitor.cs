using System;

namespace TradingPlaces.Client.Domain.Instrumentation
{
    public interface IProcessorMonitor
    {
        TimeSpan CalculateProcessingAndReset();
        bool IsAvailable { get; }
    }
}