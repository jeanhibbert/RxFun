using Microsoft.Extensions.Logging;
using TradingPlaces.Nixon;
using TradingPlaces.Server.Domain.Model;
using TradingPlaces.Server.Domain.Repositories;

namespace TradingPlaces.Server.Domain.Factories
{
    public interface ITickerDataFactory
    {
        ITickerData Create(string ticker);
    }

    public class TickerDataFactory : ITickerDataFactory
    {
        private readonly ILogger<TickerDataFactory> _logger;

        private readonly INixonService _NixonService;
        public TickerDataFactory(ILogger<TickerDataFactory> logger, INixonService NixonService)
        {
            _logger = logger;
            _NixonService = NixonService;
        }

        public ITickerData Create(string ticker)
        {
            var tickerData = new TickerData(
                ticker,
                new PriceGenerator(_logger, _NixonService));

            return tickerData;
        }
    }
}
