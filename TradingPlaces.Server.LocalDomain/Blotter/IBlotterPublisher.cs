using System.Threading.Tasks;
using TradingPlaces.Shared.DTO.Execution;

namespace TradingPlaces.Server.Blotter
{
    public interface IBlotterPublisher
    {
        Task Publish(TradeDto trade);
    }
}