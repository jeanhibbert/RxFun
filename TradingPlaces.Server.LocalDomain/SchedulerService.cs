using System.Reactive.Concurrency;

namespace TradingPlaces.Server
{
    public class SchedulerService : ISchedulerService
    {
        public IScheduler ThreadPool { get { return ThreadPoolScheduler.Instance; } }
    }
}