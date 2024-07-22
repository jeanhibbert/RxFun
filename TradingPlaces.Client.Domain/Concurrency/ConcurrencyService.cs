using System.Reactive.Concurrency;

namespace TradingPlaces.Client.Domain.Concurrency
{
    internal sealed class ConcurrencyService : IConcurrencyService
    {
        public IScheduler TaskPool
        {
            get { return TaskPoolScheduler.Default; }
        }
    }
}