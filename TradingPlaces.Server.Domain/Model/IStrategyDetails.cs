

namespace TradingPlaces.Server.Domain.Model
{
    public interface IStrategyDetails
    {
        string Ticker { get; set; }
        BuySell Instruction { get; set; }
        decimal PriceMovement { get; set; }
        int Quantity { get; set; }
    }
}
