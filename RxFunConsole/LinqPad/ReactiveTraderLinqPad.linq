<Query Kind="Program">
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\TradingPlaces.Client.Domain.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\TradingPlaces.Client.DomainPortable.dll</Reference>
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\TradingPlaces.Shared.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\TradingPlaces.Shared.dll</Reference>
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\Microsoft.AspNet.SignalR.Client.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\Microsoft.AspNet.SignalR.Client.dll</Reference>
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\Newtonsoft.Json.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\Newtonsoft.Json.dll</Reference>
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\System.Reactive.Core.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\System.Reactive.Core.dll</Reference>
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\System.Reactive.Interfaces.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\System.Reactive.Interfaces.dll</Reference>
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\System.Reactive.Linq.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\System.Reactive.Linq.dll</Reference>
  <Reference Relative="TradingPlaces.Client.Domain\bin\Debug\System.Reactive.PlatformServices.dll">&lt;MyDocuments&gt;\GitHub\ReactiveTrader\src\TradingPlaces.Client.Domain\bin\Debug\System.Reactive.PlatformServices.dll</Reference>
  <Namespace>TradingPlaces.Client.Domain</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>TradingPlaces.Client.Domain.Models</Namespace>
</Query>

void Main()
{
	var api = new ReactiveTrader();
	api.Initialize("Trader1", new []{"http://localhost:8080"});

	api.ConnectionStatusStream.DumpLive("Connection");

	var eurusd = from currencyPairs in api.ReferenceData.GetCurrencyPairsStream()
	             from currencyPair in currencyPairs
				 where currencyPair.CurrencyPair.Symbol == "EURUSD"
	             from price in currencyPair.CurrencyPair.PriceStream
				 select price;

	eurusd.Select((p,i)=> "price" + i + ":" + p.ToString()).DumpLive("EUR/USD");

	var targetPrice = 1.3625m;
	var execution = from price in eurusd.Where(price=> price.Bid.Rate < targetPrice).Take(1)
					from trade in price.Bid.ExecuteRequest(10000, "EUR")
					select trade.ToString();

	execution.DumpLive("Execution");
}
