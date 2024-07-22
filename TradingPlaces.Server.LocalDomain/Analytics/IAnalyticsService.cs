using System;
using TradingPlaces.Shared.DTO.Analytics;
using TradingPlaces.Shared.DTO.Execution;
using TradingPlaces.Shared.DTO.Pricing;

namespace TradingPlaces.Server.Analytics
{
    public interface IAnalyticsService
    {
        void Reset();
        void OnTrade(TradeDto trade);

        void OnPrice(PriceDto priceDto);
        PositionUpdatesDto CurrentPositionUpdatesDto { get; }
    }
}