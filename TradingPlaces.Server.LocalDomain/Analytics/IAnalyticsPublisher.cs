using System.Threading.Tasks;
using TradingPlaces.Shared.DTO.Analytics;

namespace TradingPlaces.Server.Analytics
{
    public interface IAnalyticsPublisher
    {
        Task Publish(PositionUpdatesDto positionUpdatesDto);
    }
}