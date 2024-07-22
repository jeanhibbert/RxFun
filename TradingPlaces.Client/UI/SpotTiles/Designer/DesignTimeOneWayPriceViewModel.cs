using System.Windows.Input;
using TradingPlaces.Client.Domain.Models;
using TradingPlaces.Client.Domain.Models.Pricing;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.SpotTiles.Designer
{
    public class DesignTimeOneWayPriceViewModel : ViewModelBase, IOneWayPriceViewModel
    {
        public DesignTimeOneWayPriceViewModel(Direction direction, string bigFigures, string pips, string tenthOfPip)
        {
            Direction = direction;
            BigFigures = bigFigures;
            Pips = pips;
            TenthOfPip = tenthOfPip;
        }

        public Direction Direction { get; private set; }
        public string BigFigures { get; private set; }
        public string Pips { get; private set; }
        public string TenthOfPip { get; private set; }
        public ICommand ExecuteCommand { get; private set; }
        public bool IsExecuting { get; private set; }
        public bool IsStale { get; private set; }
        public SpotTileExecutionMode ExecutionMode { get; set; }

        public void OnPrice(IExecutablePrice executablePrice)
        {
        }

        public void OnStalePrice()
        {
        }
    }
}