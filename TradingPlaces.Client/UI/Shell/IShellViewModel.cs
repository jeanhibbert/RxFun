using TradingPlaces.Client.UI.Blotter;
using TradingPlaces.Client.UI.Connectivity;
using TradingPlaces.Client.UI.SpotTiles;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.Shell
{
    public interface IShellViewModel : IViewModel
    {
        ISpotTilesViewModel SpotTiles { get; }
        IBlotterViewModel Blotter { get; }
        IConnectivityStatusViewModel ConnectivityStatus { get; }
    }
}
