<Query Kind="Program">
  <NuGetReference>Microsoft.Reactive.Testing</NuGetReference>
  <NuGetReference>ReactiveUI</NuGetReference>
  <NuGetReference Version="6.0.0">System.Reactive</NuGetReference>
  <NuGetReference>System.Reactive.Compatibility</NuGetReference>
  <NuGetReference>System.Reactive.Linq</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>

async Task Main()
{
}

public class TradeStrategyRepository
{
	private readonly Subject<ITickerData> _tickerDataStream = new Subject<ITickerData>();
	private readonly Dictionary<string, ITickerData> _tickerDataStore = new Dictionary<string, ITickerData>();
	
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
				//_logger.LogInformation($"New Ticker {ticker} with price stream created - [{_tickerDataStore.Count}] total tickers");
			}
			else
			{
				//_logger.LogInformation($"Existing Ticker {ticker} with price stream provided");
			}
			return tickerData;
		}
	}
}

public interface ITickerDataFactory
{
	ITickerData Create(string ticker);
}


public class TickerDataFactory : ITickerDataFactory
{
	private readonly ILogger<TickerDataFactory> _logger;

	private readonly INixonService _NixonService;
	public TickerDataFactory(ILogger<TickerDataFactory> logger, INixonService NixonService)
	{
		_logger = logger;
		_NixonService = NixonService;
	}

	public ITickerData Create(string ticker)
	{
		var tickerData = new TickerData(
			ticker,
			new PriceGenerator(_logger, _NixonService));

		return tickerData;
	}
}


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

	public string Ticker { get; private set; }

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
