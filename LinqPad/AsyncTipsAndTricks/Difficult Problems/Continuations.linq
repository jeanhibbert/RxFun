<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public class PCQueue
{
	readonly object _locker = new object();
	Task _tail = Task.FromResult (true);
	
	public Task Enqueue (Action action)
	{
		lock (_locker)
			return _tail = _tail.ContinueWith (ant => action());
	}

	public Task<TResult> Enqueue<TResult> (Func<TResult> func)
	{
		lock (_locker)
			return (Task<TResult>) (_tail = _tail.ContinueWith (ant => func()));
	}
}

void Main()
{
	var pcQ = new PCQueue();
	pcQ.Enqueue (() => SyncMethod (1)).Dump ("Task 1");
	pcQ.Enqueue (() => SyncMethod (2)).Dump ("Task 2");
	pcQ.Enqueue (() => SyncMethod (3)).Dump ("Task 3");
}

int SyncMethod (int i) { Thread.Sleep(1000); return i; }