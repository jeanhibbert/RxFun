using System.Reactive.Concurrency;

namespace TradingPlaces.Server
{
    public interface ISchedulerService
    {
        IScheduler ThreadPool { get; } 
    }
}