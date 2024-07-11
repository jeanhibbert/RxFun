using Microsoft.Extensions.Logging;
using System;
using TradingPlaces.Nixon;
using TradingPlaces.Server.Domain.Model;

namespace TradingPlaces.Server.Domain.Services
{
    public interface ITradeExecutionService
    {
        void NixonInstructionExecutionRetry(ITradeStrategy strategy);
    }

    public class TradeExecutionService : ITradeExecutionService
    {
        private readonly INixonService _NixonService;
        private readonly ILogger<TradeExecutionService> _logger;

        public TradeExecutionService(INixonService NixonService, ILogger<TradeExecutionService> logger)
        {
            _NixonService = NixonService;
            _logger = logger;
        }


        private int _maxExecutionAttempts = 5;

        public void NixonInstructionExecutionRetry(ITradeStrategy strategy)
        {
            bool tradeExecutedSuccessfully = false;
            int executionAttempts = 0;
            while (!tradeExecutedSuccessfully)
            {
                try
                {
                    executionAttempts++;
                    if (strategy.StrategyDetails.Instruction == BuySell.Buy)
                    {
                        _NixonService.Buy(strategy.StrategyDetails.Ticker, strategy.StrategyDetails.Quantity);
                        _logger?.LogInformation($"BUY Strategy discovered : {strategy.ToString()} - {strategy.Execute()} - ExecutionAttempts={executionAttempts}");
                        tradeExecutedSuccessfully = true;
                    }
                    if (strategy.StrategyDetails.Instruction == BuySell.Sell)
                    {
                        _NixonService.Sell(strategy.StrategyDetails.Ticker, strategy.StrategyDetails.Quantity);
                        _logger?.LogInformation($"SELL Strategy discovered : {strategy.ToString()} - {strategy.Execute()} - ExecutionAttempts={executionAttempts}");
                        tradeExecutedSuccessfully = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning($"Trade execution failure", ex);
                    if (executionAttempts == _maxExecutionAttempts)
                    {
                        _logger?.LogWarning($"Trade execution failure attempt limit [{_maxExecutionAttempts}] reached {strategy.ToString()}", ex);
                        strategy.ExecutionFailureReason = ex.Message;
                        return;
                    }
                }
            }
        }
    }
}
