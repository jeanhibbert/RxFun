using System.Windows.Input;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.SpotTiles
{
    public interface ISpotTileConfigViewModel : IViewModel
    {
        ICommand StandardCommand { get; }
        ICommand DropFrameCommand { get; }
        ICommand ConflateCommand { get; }
        ICommand ConstantRateCommand { get; }
        ICommand AsyncCommand { get; }
        ICommand SyncCommand { get; }
        SpotTileSubscriptionMode SubscriptionMode { get; }
        SpotTileExecutionMode ExecutionMode { get; }

    }
}