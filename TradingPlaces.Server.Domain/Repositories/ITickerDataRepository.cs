using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using TradingPlaces.Server.Domain.Factories;
using TradingPlaces.Server.Domain.Model;

namespace TradingPlaces.Server.Domain.Repositories
{
    public interface ITickerDataRepository
    {
        IObservable<ITickerData> GetTickerDataStream();
        ITickerData RegisterTicker(string ticker);
        void DisposeUnusedTickers();
    }

    public class TickerDataRepository : ITickerDataRepository
    {
        private readonly Subject<ITickerData> _tickerDataStream = new Subject<ITickerData>();
        private readonly Dictionary<string, ITickerData> _tickerDataStore = new Dictionary<string, ITickerData>();
        private readonly ITickerDataFactory _tickerDataFactory;
        private readonly ILogger<TickerDataRepository> _logger;
        private readonly ITradeStrategyRepository _tradeStrategyRepository;

        public TickerDataRepository(ITickerDataFactory tickerDataFactory, 
            ITradeStrategyRepository tradeStrategyRepository,
            ILogger<TickerDataRepository> logger)
        {
            _tickerDataFactory = tickerDataFactory;
            _tradeStrategyRepository = tradeStrategyRepository;
            _logger = logger;
        }

        public IObservable<ITickerData> GetTickerDataStream()
        {
            return _tickerDataStream;
        }

        public ITickerData RegisterTicker(string ticker)
        {
            lock (_tickerDataStore)
            {
                ITickerData tickerData;
                if (!_tickerDataStore.TryGetValue(ticker, out tickerData))
                {
                    tickerData = _tickerDataFactory.Create(ticker);
                    _tickerDataStore.Add(ticker, tickerData);
                    _tickerDataStream.OnNext(tickerData);
                    _logger?.LogInformation($"New Ticker {ticker} with price stream created - [{_tickerDataStore.Count}] total tickers");
                }
                else
                {
                    _logger?.LogInformation($"Existing Ticker {ticker} with price stream provided");
                }
                return tickerData;
            }
        }

        public void DisposeUnusedTickers()
        {
            var tickersToRemove = (from ts in _tradeStrategyRepository.TradeStrategies
                                   join td in _tickerDataStore.Values on ts.StrategyDetails.Ticker equals td.Ticker
                                   group ts by ts.StrategyDetails.Ticker into tickerGroup
                                   select new
                                   {
                                       Ticker = tickerGroup.Key,
                                       AwaitingExecutionCount = tickerGroup.Where(x => !x.HasExecuted && x.ExecutionFailureReason == null).Count()
                                   }).ToList()
                                  .Where(x => x.AwaitingExecutionCount == 0)
                                  .Select(x => x.Ticker);
                                  
            foreach(var ticker in tickersToRemove)
            {
                _tickerDataStore[ticker].Dispose();
                _tickerDataStore.Remove(ticker);
            }
        }
    }
}
