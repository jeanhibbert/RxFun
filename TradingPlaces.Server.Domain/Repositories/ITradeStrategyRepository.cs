using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using TradingPlaces.Server.Domain.Model;

namespace TradingPlaces.Server.Domain.Repositories
{
    public interface ITradeStrategyRepository
    {
        List<ITradeStrategy> TradeStrategies { get; }

        IList<string> AddTradeStrategy(IStrategyDetails strategyDetails, IObservable<IPrice> startingPrice);

        bool RemoveStrategy(string strategyId);
    }

    public class TradeStrategyRepository : ITradeStrategyRepository
    {
        public List<ITradeStrategy> TradeStrategies { get; } = new List<ITradeStrategy>();

        public IList<string> AddTradeStrategy(IStrategyDetails strategyDetails, IObservable<IPrice> priceStream)
        {
            lock (TradeStrategies)
            {
                TradeStrategies.Add(new TradeStrategy(strategyDetails, priceStream));
                return TradeStrategies.Select(x => x.TradeStrategyId).ToList();
            }
        }

        public bool RemoveStrategy(string strategyId)
        {
            lock (TradeStrategies)
            {
                var tradeStrategyToRemove = TradeStrategies.SingleOrDefault(x => x.TradeStrategyId == strategyId);
                if (tradeStrategyToRemove != null)
                {
                    TradeStrategies.Remove(tradeStrategyToRemove);
                    return true;
                }
                return false;
            }
        }
    }
}
