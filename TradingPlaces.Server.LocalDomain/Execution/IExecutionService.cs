using System.Threading.Tasks;
using TradingPlaces.Shared.DTO.Execution;

namespace TradingPlaces.Server.Execution
{
    public interface IExecutionService
    {
        Task<TradeDto> Execute(TradeRequestDto tradeRequest, string username);
    }
}