using Microsoft.AspNet.SignalR.Hubs;

namespace TradingPlaces.Server.Transport
{
    public interface IContextHolder
    {
        IHubCallerConnectionContext PricingHubClient { get; set; }
        IHubCallerConnectionContext BlotterHubClients { get; set; }
        IHubCallerConnectionContext ReferenceDataHubClients { get; set; }
        IHubCallerConnectionContext AnalyticsHubClients { get; set; }
    }
}