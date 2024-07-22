using System.Reactive.Concurrency;

namespace TradingPlaces.Client.Concurrency
{
    public interface IConcurrencyService
    {
        IScheduler Dispatcher { get; }
        IScheduler TaskPool { get; }
    }
}