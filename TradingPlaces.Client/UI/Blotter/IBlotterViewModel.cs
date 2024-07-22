using System.Collections.ObjectModel;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.Blotter
{
    public interface IBlotterViewModel : IViewModel
    {
        ObservableCollection<ITradeViewModel> Trades { get; } 
    }
}
