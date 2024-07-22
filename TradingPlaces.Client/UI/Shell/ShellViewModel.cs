using TradingPlaces.Client.UI.Blotter;
using TradingPlaces.Client.UI.Connectivity;
using TradingPlaces.Client.UI.SpotTiles;
using TradingPlaces.Shared.UI;
using PropertyChanged;

namespace TradingPlaces.Client.UI.Shell
{
    [ImplementPropertyChanged]
    public class ShellViewModel : ViewModelBase, IShellViewModel
    {
        public ISpotTilesViewModel SpotTiles { get; private set; }
        public IBlotterViewModel Blotter { get; private set; }
        public IConnectivityStatusViewModel ConnectivityStatus { get; private set; }

        public ShellViewModel(ISpotTilesViewModel spotTiles, IBlotterViewModel blotter, IConnectivityStatusViewModel connectivityStatusViewModel)
        {
            SpotTiles = spotTiles;
            Blotter = blotter;
            ConnectivityStatus = connectivityStatusViewModel;
        }
    }
}
