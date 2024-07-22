using System;
using TradingPlaces.Client.Domain.Models.Execution;
using TradingPlaces.Client.Domain.Repositories;
using TradingPlaces.Shared.Extensions;

namespace TradingPlaces.Client.Domain.Models.Pricing
{
    internal class ExecutablePrice : IExecutablePrice
    {
        private readonly IExecutionRepository _executionRepository;

        public ExecutablePrice(Direction direction, decimal rate, IExecutionRepository executionRepository)
        {
            _executionRepository = executionRepository;
            Direction = direction;
            Rate = rate;
        }

        public IObservable<IStale<ITrade>> ExecuteRequest(long notional, string dealtCurrency)
        {
            return _executionRepository.ExecuteRequest(this, notional, dealtCurrency);
        }

        public Direction Direction { get; private set; }
        public decimal Rate { get; private set; }
        public IPrice Parent { get; internal set; }
    }
}