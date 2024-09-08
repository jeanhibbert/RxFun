<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public class PCQueue : IDisposable
{
	BlockingCollection<Task> _taskQ = new BlockingCollection<Task>();
	
	public PCQueue()
	{
		Task.Run (() => Consume());
	}
	
	public Task Enqueue (Action action)
	{
		var task = new Task (action);
		_taskQ.Add (task);
		return task;
	}
	
	public Task<TResult> Enqueue<TResult> (Func<TResult> func)
	{
		var task = new Task<TResult> (func);
		_taskQ.Add (task);
		return task;
	}
	
	void Consume()
	{
		foreach (var task in _taskQ.GetConsumingEnumerable())
			task.RunSynchronously();
	}
	
	public void Dispose() 
	{
		_taskQ.CompleteAdding();
	}
}

void Main()
{
	using (var pcQ = new PCQueue())
	{
		pcQ.Enqueue (() => SyncMethod (1)).Dump ("Task 1");
		pcQ.Enqueue (() => SyncMethod (2)).Dump ("Task 2");
		pcQ.Enqueue (() => SyncMethod (3)).Dump ("Task 3");
	}	
}

int SyncMethod (int i) { Thread.Sleep(1000); return i; }