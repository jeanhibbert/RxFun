using TradingPlaces.Server.Domain.Model;

namespace TradingPlaces.Server.Domain.Dtos
{
    public class StrategyDetailsDto : IStrategyDetails
    {
        public string Ticker { get; set; }
        public BuySell Instruction { get; set; }
        public decimal PriceMovement { get; set; }
        public int Quantity { get; set; }
    }
}
public enum BuySell
{
    Buy,
    Sell
}