using TradingPlaces.Client.Domain.Models.Pricing;

namespace TradingPlaces.Client.Domain.Instrumentation
{
    public interface IPriceLatencyRecorder
    {
        void OnRendered(IPrice price);
        void OnReceived(IPrice price);
        Statistics CalculateAndReset();
    }
}