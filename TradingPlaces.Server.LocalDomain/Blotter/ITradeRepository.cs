using System.Collections.Generic;
using TradingPlaces.Shared.DTO.Execution;

namespace TradingPlaces.Server.Blotter
{
    public interface ITradeRepository
    {
        void Reset();
        void StoreTrade(TradeDto trade);
        IList<TradeDto> GetAllTrades();

    }
}