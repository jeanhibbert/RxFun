using System.Collections.ObjectModel;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.SpotTiles
{
    public interface ISpotTilesViewModel : IViewModel
    {
        ObservableCollection<ISpotTileViewModel> SpotTiles { get; } 
    }
}
