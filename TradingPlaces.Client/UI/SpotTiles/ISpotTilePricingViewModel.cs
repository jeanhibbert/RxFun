using System;
using TradingPlaces.Client.Domain.Models.Execution;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.SpotTiles
{
    public interface ISpotTilePricingViewModel : IViewModel, IDisposable
    {
        SpotTileSubscriptionMode SubscriptionMode { get; set; }
        SpotTileExecutionMode ExecutionMode { get; set; }
        string Symbol { get; }
        IOneWayPriceViewModel Bid { get; }
        IOneWayPriceViewModel Ask { get; }
        string Notional { get; set; }
        string Spread { get; }
        string DealtCurrency { get; }
        PriceMovement Movement { get; }
        string SpotDate { get; }
        bool IsSubscribing { get; }
        bool IsStale { get; }
        void OnTrade(ITrade trade);
        void OnExecutionError(string message);
    }
}
