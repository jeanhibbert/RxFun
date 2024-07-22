using System;
using System.Threading.Tasks;
using TradingPlaces.Server.Transport;
using TradingPlaces.Shared;
using TradingPlaces.Shared.DTO.Analytics;

namespace TradingPlaces.Server.Analytics
{
    public class AnalyticsPublisher : IAnalyticsPublisher
    {
        private readonly IContextHolder _contextHolder;

        public AnalyticsPublisher(IContextHolder contextHolder)
        {
            _contextHolder = contextHolder;
        }

        public async Task Publish(PositionUpdatesDto positionUpdatesDto)
        {
            var context = _contextHolder.AnalyticsHubClients;
            if (context == null)
                return;

            try
            {
                await context.Group(ServiceConstants.Server.AnalyticsGroup).OnNewAnalytics(positionUpdatesDto);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}