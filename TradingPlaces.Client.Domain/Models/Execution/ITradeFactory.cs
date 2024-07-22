using TradingPlaces.Shared.DTO.Execution;

namespace TradingPlaces.Client.Domain.Models.Execution
{
    interface ITradeFactory
    {
        ITrade Create(TradeDto trade);
    }
}