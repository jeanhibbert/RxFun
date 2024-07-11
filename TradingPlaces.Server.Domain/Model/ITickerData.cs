using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using TradingPlaces.Server.Domain.Repositories;

namespace TradingPlaces.Server.Domain.Model
{
    public interface ITickerData : IDisposable
    {
        string Ticker { get; }
        IObservable<IPrice> PriceStream { get; }
    }

    public class TickerData : ITickerData
    {

        private readonly Lazy<IObservable<IPrice>> _lazyPriceStream;
        private readonly IPriceGenerator _priceGenerator;

        public TickerData(string ticker, IPriceGenerator priceGenerator)
        {
            Ticker = ticker;
            _priceGenerator = priceGenerator;
            _lazyPriceStream = new Lazy<IObservable<IPrice>>(() => CreatePriceStream(priceGenerator));
        }

        public string Ticker{ get; private set; }

        public IObservable<IPrice> PriceStream { get { return _lazyPriceStream.Value; } }

        public void Dispose()
        {
            if (_priceGenerator != null)
                _priceGenerator.Dispose();
        }

        private IObservable<IPrice> CreatePriceStream(IPriceGenerator priceRepository)
        {
            return priceRepository.GetPriceStream(this)
                .Publish()
                .RefCount();
        }
    }
}
