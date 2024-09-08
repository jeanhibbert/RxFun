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

	public Task Enqueue (Func<Task> action)
	{
		lock (_locker)
			return _tail = _tail.
	}	

	public Task<TResult> Enqueue<TResult> (Func<Task<TResult>> func)
	{
		lock (_locker)
			return (Task<TResult>) (_tail = _tail.
	}
}

void Main()
{
	var pcQ = new PCQueue();
	pcQ.Enqueue (() => SyncMethod (1)).Dump ("Task 1");
	pcQ.Enqueue (() => SyncMethod (2)).Dump ("Task 2");
	pcQ.Enqueue (() => SyncMethod (3)).Dump ("Task 3");
	
	pcQ.Enqueue (() => AsyncMethod (4)).Dump ("Task 4");
	pcQ.Enqueue (() => AsyncMethod (5)).Dump ("Task 5");
	pcQ.Enqueue (() => AsyncMethod (6)).Dump ("Task 6");
}

           int  SyncMethod  (int i) {     Thread.Sleep(1000); return i; }
async Task<int> AsyncMethod (int i) { await Task.Delay(1000); return i; }

#region Extensions
public static class Extensions
{
	public static Task<T> Catch<T,TError> (this Task<T> task, Func<TError,T> onError) where TError : Exception
	{
		var tcs = new TaskCompletionSource<T> ();
		
		task.ContinueWith (ant =>
		{
			if (task.IsFaulted && task.Exception.InnerException is TError)
				tcs.SetResult (onError ((TError) task.Exception.InnerException));
			else if (ant.IsCanceled)
				tcs.SetCanceled ();
			else if (task.IsFaulted)
				tcs.SetException (ant.Exception.InnerException);
			else
				tcs.SetResult (ant.Result);
		});
		return tcs.Task;
	}	
	
	public static Task<T> Catch<T,TError> (this Task<T> task, T onError) where TError : Exception
	{
		return task.Catch<T,TError> (ex => onError);
	}
	
	public static Task<T> Catch<T,TError> (this Task task, T onError) where TError : Exception
	{
		return task.ToTaskOfT<T>().Catch<T,TError> (onError);
	}

	public static Task Catch (this Task task)
	{
		return task.Catch<object,Exception> (null);
	}
	
	public static Task<T> Catch<T> (this Task<T> task, T valueIfError = default(T))
	{
		return task.Catch<T,Exception> (valueIfError);
	}

	public async static Task<T> ToTaskOfT<T> (this Task t)
	{
		await t;
		return default(T);
	}
	
	// Stephen Toub's monadic bind
	public static async Task Then(this Task task, Func<Task> continuation)
	{
		await task;
		await continuation();
	}
	
	public static async Task<TNewResult> Then<TNewResult>(this Task task, Func<Task<TNewResult>> continuation)
	{
		await task;
		return await continuation();
	}
	
	public static async Task Then<TResult>(this Task<TResult> task, Func<TResult,Task> continuation)
	{
		await continuation(await task);
	}
	
	public static async Task<TNewResult> Then<TResult, TNewResult>(this Task<TResult> task, Func<TResult, Task<TNewResult>> continuation)
	{
		return await continuation(await task);
	}
}
#endregion