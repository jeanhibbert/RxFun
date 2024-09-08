<Query Kind="Program">
  <Reference>&lt;ApplicationData&gt;\LINQPad\Samples\LINQ in Action\LinqInAction.LinqBooks.Common.dll</Reference>
  <NuGetReference>System.Reactive.Linq</NuGetReference>
  <NuGetReference>System.Reactive.PlatformServices</NuGetReference>
  <Namespace>LinqInAction.LinqBooks.Common</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{

	var subscription = Observable.Create<Price>(observer => {

		Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>
		{
			$"publishing... {x}".Dump();
			observer.OnNext(new Price { Total = x });
		});

		return Disposable.Create( () => "Disposed!".Dump());
	
	}).Publish().RefCount();
	
	
	var sub1 = subscription.ObserveOn(CurrentThreadScheduler.Instance).
	SubscribeOn(NewThreadScheduler.Default).Subscribe(new MyObserver("Test1"));
	
	Thread.Sleep(10000);

	var sub2 = subscription.SubscribeOn(CurrentThreadScheduler.Instance).
	ObserveOn(NewThreadScheduler.Default).Subscribe(new MyObserver("Test2"));

	Thread.Sleep(10000);

	sub1.Dispose();
	
	Thread.Sleep(10000);

	sub2.Dispose();
}

public class MyObserver : IObserver<Price>
{
	public string Name { get; }
	public MyObserver(string name)
	{
		Name = name;
	}
	
	public void OnNext(Price value)
	{
		$"{Name} recieved a price - {value} - {Thread.CurrentThread.ManagedThreadId}".Dump();
		
	}
	public void OnError (Exception ex)
	{
		"Error!!".Dump();
	}
	public void OnCompleted()
	{
		"Completed!".Dump();
	}
}

public class Price
{
	public long Total { get; set; }
	public override string ToString() => $"Price is {Total}";
}

// Define other methods and classes here
