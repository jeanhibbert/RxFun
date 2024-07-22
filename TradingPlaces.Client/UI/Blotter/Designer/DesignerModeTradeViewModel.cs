using System;
using TradingPlaces.Client.Domain.Models;
using TradingPlaces.Shared.UI;

namespace TradingPlaces.Client.UI.Blotter.Designer
{
    public class DesignerModeTradeViewModel : ViewModelBase, ITradeViewModel
    {
        public decimal SpotRate { get; set; }
        public string Notional { get; set; }
        public Direction Direction { get; set; }
        public string CurrencyPair { get; set; }
        public string TradeId { get; set; }
        public DateTime TradeDate { get; set; }
        public string TradeStatus { get; set; }
        public string TraderName { get; set; }
        public DateTime ValueDate { get; set; }
        public bool IsNewTrade { get; set; }
    }
}