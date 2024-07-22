using System.Threading.Tasks;
using TradingPlaces.Shared.DTO.ReferenceData;

namespace TradingPlaces.Server.ReferenceData
{
    public interface ICurrencyPairUpdatePublisher
    {
        Task AddCurrencyPair(CurrencyPairDto ccyPair);
        Task RemoveCurrencyPair(CurrencyPairDto ccyPair);
    }
}