using System;
using System.Reactive;

namespace TradingPlaces.Client.Concurrency
{
    public interface IConstantRatePump
    {
        IObservable<Unit> Tick { get; } 
    }
}