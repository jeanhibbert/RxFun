using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;

namespace TradingPlaces.Server.Domain.Model
{
    public interface ITradeStrategy
    {
        string TradeStrategyId { get; }

        bool IsReadyToExecute(IPrice currentPrice);

        IStrategyDetails StrategyDetails { get; }

        IPrice StartingPrice { get; }

        IPrice TradePrice { get; }

        string Execute();

        decimal? TargetPriceValue { get; }

        bool HasExecuted { get; }
        
        string ExecutionFailureReason { get; set; }
    }

    public class TradeStrategy : ITradeStrategy
    {
        private readonly IObservable<IPrice> _priceStream;

        public TradeStrategy(IStrategyDetails strategyDetails, 
            IObservable<IPrice> startingPriceStream)
        {
            TradeStrategyId = Guid.NewGuid().ToString();
            StrategyDetails = strategyDetails;
            _priceStream = startingPriceStream;
            _priceStream.Take(1).Subscribe(price => {
                StartingPrice = price;
                SetTargetPriceValue();
            });
        }

        private void SetTargetPriceValue()
        {
            var startingPriceValue = StartingPrice.Price;
            var priceDifference = StrategyDetails.PriceMovement / 100 * startingPriceValue;
            TargetPriceValue = StrategyDetails.Instruction == BuySell.Buy
                ? startingPriceValue - priceDifference : startingPriceValue + priceDifference;
        }

        public string TradeStrategyId { get; private set; }

        public IStrategyDetails StrategyDetails { get; private set; }

        public IPrice StartingPrice { get; private set; }

        public IPrice TradePrice { get; private set; }

        public decimal? TargetPriceValue { get; private set; }

        public bool HasExecuted { get; private set; }

        public string ExecutionFailureReason { get; set; }

        public string Execute()
        {
            HasExecuted = true;
            return  $"[TRADE EXECUTED] - {StrategyDetails.Quantity} of {StrategyDetails.Ticker} : {StrategyDetails.Instruction}";
        }

        public bool IsReadyToExecute(IPrice currentPrice)
        {
            if (!TargetPriceValue.HasValue)
                return false;

            if (StrategyDetails.Instruction == BuySell.Buy)
            {
                if (currentPrice.Price <= TargetPriceValue)
                {
                    TradePrice = currentPrice;
                    return true;
                }
            }
            else
            {
                if (currentPrice.Price >= TargetPriceValue)
                {
                    TradePrice = currentPrice;
                    return true;
                }
            }

            return false;
        }
    }
}