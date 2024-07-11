using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TradingPlaces.Nixon;
using TradingPlaces.Server.Domain.Factories;
using TradingPlaces.Server.Domain.Model;

namespace TradingPlaces.Server.Domain.Repositories
{
    public interface IPriceGenerator : IDisposable
    {
        IObservable<IPrice> GetPriceStream(ITickerData tickerData);
    }
         
    public class PriceGenerator : IPriceGenerator
    {

        private IDisposable _subscription = null;
        private ISubject<PriceDto> _priceStream = new ReplaySubject<PriceDto>(1);
        private readonly ILogger<TickerDataFactory> _logger;
        private readonly INixonService _NixonService;
        private readonly int _priceCheckInterval = ServiceConstants.Server.PriceStreamIntervalMilliseconds;

        public PriceGenerator(ILogger<TickerDataFactory> logger, INixonService NixonService)
        {
            _logger = logger;
            _NixonService = NixonService;
        }

        public IObservable<IPrice> GetPriceStream(ITickerData tickerData)
        {
            IObservable<long> source = Observable.Interval(TimeSpan.FromMilliseconds(_priceCheckInterval));
            _subscription = source.Subscribe(
                x =>
                {
                    var newPrice = GetChange(tickerData.Ticker);
                    if (newPrice.HasValue)
                    {
                        var price = new PriceDto(tickerData.Ticker, tickerData, newPrice.Value);
                        _priceStream.OnNext(price);
                    }
                },
                ex =>
                {
                    _logger?.LogWarning("Error loading price from Nixon Api: {0}", ex.Message);
                },
                () => _logger?.LogWarning("Stopped loading prices from Nixon Api"));

            return _priceStream;
        }

        private decimal? GetChange(string ticker)
        {
            try
            {
                return _NixonService.GetQuote(ticker);
            }
            catch(Exception ex)
            {
                _logger?.LogWarning($"Nixon API threw exception : {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            if (_subscription != null)
                _subscription.Dispose();
        }


    }
}
