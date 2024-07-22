using System.Windows.Input;
using TradingPlaces.Client.Domain.Models;
using TradingPlaces.Client.Domain.Models.Pricing;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.SpotTiles
{
    public interface IOneWayPriceViewModel : IViewModel
    {
        Direction Direction { get; }
        string BigFigures { get; }
        string Pips { get; }
        string TenthOfPip { get; }
        ICommand ExecuteCommand { get; }
        bool IsExecuting { get; }
        void OnPrice(IExecutablePrice executablePrice);
        void OnStalePrice();
        SpotTileExecutionMode ExecutionMode { get; set; }
    }
}
