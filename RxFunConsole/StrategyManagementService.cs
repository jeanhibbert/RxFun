using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TradingPlaces.Server.Domain.Repositories;
using TradingPlaces.Server.Domain.Services;
using TradingPlaces.Server.Domain.Dtos;
using TradingPlaces.Server.Domain;

namespace TradingPlaces.WebApi.Services
{
    internal class StrategyManagementService : IStrategyManagementService
    {
        private const int TickFrequencyMilliseconds = ServiceConstants.Server.TickFrequencyMilliseconds;
        private readonly ILogger<StrategyManagementService> _logger;
        private readonly ITickerDataRepository _tickerDataRepository;
        private readonly ITradeStrategyRepository _tradeStrategyRepository;
        private readonly ITradeExecutionService _tradeExecutionService;

        public StrategyManagementService(ILogger<StrategyManagementService> logger, 
            ITickerDataRepository tickerDataRepository,
            ITradeStrategyRepository tradeStrategyRepository,
            ITradeExecutionService tradeExecutionService)
        {
            _logger = logger;
            _tickerDataRepository = tickerDataRepository;
            _tradeStrategyRepository = tradeStrategyRepository;
            _tradeExecutionService = tradeExecutionService;
        }

        public IList<string> RegisterStrategy(StrategyDetailsDto strategyDetails)
        {
            //if (!strategyDetails.Ticker.IsValidTicker())
            //    throw new ArgumentException("Invalid ticker provided");
            
            return _tradeStrategyRepository.AddTradeStrategy(strategyDetails, 
                _tickerDataRepository.RegisterTicker(strategyDetails.Ticker).PriceStream);
        }

        public bool UnregisterStrategy(string strategyId)
        {
            return _tradeStrategyRepository.RemoveStrategy(strategyId);
        }

        protected Task CheckStrategies()
        {
            _logger?.LogInformation($"Checking strategies...");

            var tickerPriceStream = from tickerData in _tickerDataRepository.GetTickerDataStream()
                                        //where tickerData.Ticker == "MSFT"
                                        from price in tickerData.PriceStream
                                        select price;
                     
            var validTradeStrategy = from price in tickerPriceStream
                         from tradeStrategy in _tradeStrategyRepository.TradeStrategies
                         where tradeStrategy.HasExecuted == false 
                            && tradeStrategy.ExecutionFailureReason == null
                            && tradeStrategy.StrategyDetails.Ticker == price.Ticker
                            && tradeStrategy.IsReadyToExecute(price)
                         select tradeStrategy;

            validTradeStrategy.Subscribe(strategy =>
            {
                _tradeExecutionService.NixonInstructionExecutionRetry(strategy);
            });

            LogTradeStrategyStatus();
            _tickerDataRepository.DisposeUnusedTickers();

            return Task.CompletedTask;
        }

        private void LogTradeStrategyStatus()
        {
            var outstandingTotal = _tradeStrategyRepository.TradeStrategies.Where(x => !x.HasExecuted).Count();
            var executedTotal = _tradeStrategyRepository.TradeStrategies.Where(x => x.HasExecuted).Count();
            var failedTotal = _tradeStrategyRepository.TradeStrategies.Where(x => x.ExecutionFailureReason != null).Count();
            _logger?.LogInformation($"Strategies checked... OUSTANDING: {outstandingTotal} | EXECUTED: {executedTotal} | FAILED: {failedTotal}");
        }

        
    }
}
