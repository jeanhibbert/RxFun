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
	//Test1();
	//ReplaySubjectBufferExample();	
	//await TestPublishRefCount();

	//await TestAsynEnum();
	//"done".Dump();
	//Console.ReadLine();
	
	
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


public async Task TestAsynEnum()
{
	//var consumer = new AsyncFunctionConsumer();
	//var observable = consumer.ConsumeAsyncFunction(MyAsyncFunction);

	IAsyncEnumerable<int> asyncEnumerable = GetItemsAsync();

	IObservable<int> observable = Observable.Create<int>(async (observer, cancellationToken) =>
		{
			await foreach (var item in asyncEnumerable.WithCancellation(cancellationToken))
			{
				observer.OnNext(item);
			}

			observer.OnCompleted();
		});

	"test".Dump();
	observable.Subscribe(data =>
	{
		Console.WriteLine($"Received data: {data}");
	}, error => { }, () => "Yebo completed".Dump());

	Console.ReadLine();
}

public class AsyncFunctionConsumer
{
	public IObservable<T> ConsumeAsyncFunction<T>(Func<Task<T>> asyncFunction)
	{
		return Observable.FromAsync(asyncFunction);
	}
}

public async Task<string> MyAsyncFunction()
{
	await Task.Delay(1000);  // Simulate some asynchronous operation
	return "Data from async function";
}

public async IAsyncEnumerable<int> GetItemsAsync()
{
	"2".Dump();
	await Task.Delay(1000);
	"3".Dump();
	yield return 1;

"4".Dump();
	await Task.Delay(1000);
	yield return 2;

"5".Dump();
	await Task.Delay(1000);
	yield return 3;
	"6".Dump();
}


public void Test1()
{
	IObservable<int> source = Observable.Generate(
	 	0, i => i < 5,
		 i => i + 1,
		 i => i * i, i => TimeSpan.FromSeconds(i));
	using (source.Subscribe(
	x => Console.WriteLine("OnNext: {0}", x),
	ex => Console.WriteLine("OnError: {0}", ex.Message),
	() => Console.WriteLine("OnCompleted")
	))
	{
		Console.WriteLine("Press ENTER to unsubscribe...");
		Console.ReadLine();
	};
}

public void ReplaySubjectBufferExample()
{
var bufferSize = 2;
	//var subject = new ReplaySubject<string>(bufferSize);
	var subject = new ReplaySubject<string>(bufferSize);
	subject.OnNext("a");
	subject.OnNext("b");
	subject.OnNext("c");
	subject.Subscribe(x =>
	{
		Console.WriteLine(x); 
		Thread.Sleep(2000);
		}
	);
		
	subject.OnNext("d");
}

public async Task TestPublishRefCount()
{
	var coldObservable = Observable.Interval(TimeSpan.FromSeconds(1))
	.Do(value => Console.WriteLine($"Producing value: {value}"))
	.Publish();

	var connectedObservable = coldObservable.RefCount();

	var subscription1 = connectedObservable.Subscribe(value =>
	{
		Console.WriteLine($"Subscriber 1 received value: {value}");
	});

	await Task.Delay(TimeSpan.FromSeconds(2));

	var subscription2 = connectedObservable.Subscribe(value =>
	{
		Console.WriteLine($"Subscriber 2 received value: {value}");
	});

	await Task.Delay(TimeSpan.FromSeconds(3));

	subscription1.Dispose();

	await Task.Delay(TimeSpan.FromSeconds(2));

	subscription2.Dispose();
}
