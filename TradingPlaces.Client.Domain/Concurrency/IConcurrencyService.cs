using System.Reactive.Concurrency;

namespace TradingPlaces.Client.Domain.Concurrency
{
    internal interface IConcurrencyService
    {
         IScheduler TaskPool { get; }
    }
}