using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingPlaces.Nixon;
using TradingPlaces.Server.Domain.Dtos;
using TradingPlaces.Server.Domain.Factories;
using TradingPlaces.Server.Domain.Model;
using TradingPlaces.Server.Domain.Repositories;
using TradingPlaces.Server.Domain.Services;
using TradingPlaces.WebApi.Services;

namespace RxFunConsole;

internal class Program
{

    private readonly IStrategyManagementService _strategyManagementService;

    static void Main(string[] args)
    {
        var strategyDetails = new StrategyDetailsDto
        {
            Ticker = "MSFT",
            Instruction = BuySell.Buy,
            PriceMovement = 1,
            Quantity = 1
        };

        var strategyManagementService = new StrategyManagementService(
            null,
            
            new TickerDataRepository(
                new TickerDataFactory(
                    null, 
                    new NixonService()), 
                new TradeStrategyRepository(), null),
            
            new TradeStrategyRepository(),
            
            new TradeExecutionService(
                new NixonService()
                , null)
        );

        var strategyIds = strategyManagementService.RegisterStrategy(strategyDetails);

        //var id = Guid.NewGuid().ToString();
        //var success = strategyManagementService.UnregisterStrategy(id);

        Console.WriteLine("Hello, World!");
        Console.ReadKey();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IStrategyManagementService, StrategyManagementService>();
        services.AddSingleton<INixonService, NixonService>();
        services.AddSingleton<ITickerDataRepository, TickerDataRepository>();
        services.AddSingleton<ITradeStrategyRepository, TradeStrategyRepository>();
        services.AddSingleton<ITickerDataFactory, TickerDataFactory>();
        services.AddSingleton<ITradeExecutionService, TradeExecutionService>();
    }
}
