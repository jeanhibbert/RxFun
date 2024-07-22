using System;
using System.Reactive.Linq;
using TradingPlaces.Client.Domain.Concurrency;
using TradingPlaces.Client.Domain.Models;
using TradingPlaces.Client.Domain.Models.Execution;
using TradingPlaces.Client.Domain.Models.Pricing;
using TradingPlaces.Client.Domain.ServiceClients;
using TradingPlaces.Shared.DTO.Execution;
using TradingPlaces.Shared.Extensions;

namespace TradingPlaces.Client.Domain.Repositories
{
    internal class ExecutionRepository : IExecutionRepository
    {
        private readonly IExecutionServiceClient _executionServiceClient;
        private readonly ITradeFactory _tradeFactory;
        private readonly IConcurrencyService _concurrencyService;

        public ExecutionRepository(IExecutionServiceClient executionServiceClient, ITradeFactory tradeFactory, IConcurrencyService concurrencyService)
        {
            _executionServiceClient = executionServiceClient;
            _tradeFactory = tradeFactory;
            _concurrencyService = concurrencyService;
        }

        public IObservable<IStale<ITrade>> ExecuteRequest(IExecutablePrice executablePrice, long notional, string dealtCurrency)
        {
            return Observable.Defer(() =>
            {
                var price = executablePrice.Parent;

                var request = new TradeRequestDto
                {
                    Direction = executablePrice.Direction == Direction.BUY ? DirectionDto.Buy : DirectionDto.Sell,
                    Notional = notional,
                    SpotRate = executablePrice.Rate,
                    Symbol = price.CurrencyPair.Symbol,
                    ValueDate = price.ValueDate,
                    DealtCurrency = dealtCurrency
                };

                return _executionServiceClient.ExecuteRequest(request)
                    .Select(_tradeFactory.Create)
                    .DetectStale(TimeSpan.FromSeconds(2), _concurrencyService.TaskPool);
            });
        }
    }
}