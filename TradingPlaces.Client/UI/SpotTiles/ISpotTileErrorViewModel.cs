using System.Windows.Input;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.SpotTiles
{
    public interface ISpotTileErrorViewModel : IViewModel
    {
        string ErrorMessage { get; }
        ICommand DismissCommand { get; }
    }
}