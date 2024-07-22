using System.Threading.Tasks;
using TradingPlaces.Shared.DTO.Pricing;

namespace TradingPlaces.Server.Pricing
{
    public interface IPricePublisher
    {
        Task Publish(PriceDto price);
        long TotalPricesPublished { get; }
    }
}