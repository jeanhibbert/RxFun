<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

public static class Extensions
{
	public async static Task<TResult> WithTimeout<TResult> (this Task<TResult> task, TimeSpan timeout)
	{
		Task winner = await (Task.WhenAny (task, Task.Delay (timeout))).ConfigureAwait (false);
		if (winner != task) throw new TimeoutException ();
		return await task.ConfigureAwait (false);   // Unwrap result/re-throw
	}

	public static Task<TResult> WithTimeout<TResult> (this Task<TResult> task, int timeout)
	{
		return task.WithTimeout (TimeSpan.FromMilliseconds (timeout));
	}

	public static Task WithTimeout (this Task task, int timeout)
	{
		return task.ToTaskOfT<object>().WithTimeout (timeout);
	}

	public static Task WithTimeout (this Task task, TimeSpan timeout)
	{
		return task.ToTaskOfT<object>().WithTimeout (timeout);
	}	
	
	#region WithComputeOpportunities
	public static Task WithComputeOpportunities (this Task task, int computeOpportunities, int timeout)
	{
		return WithComputeOpportunities (task.ToTaskOfT<object> (), computeOpportunities, timeout);
	}

	public async static Task<T> WithComputeOpportunities<T> (this Task<T> task, int computeOpportunities, int timeout)
	{
		var tcs = new CancellationTokenSource();
		Task winner = await (Task.WhenAny (task, AfterComputeOpportunities (computeOpportunities, timeout, tcs.Token))).ConfigureAwait (false);
		if (winner != task) throw new TimeoutException ();
		tcs.Cancel();
		return await task.ConfigureAwait (false);   // Unwrap result/re-throw
	}

	async static Task AfterComputeOpportunities (int computeOpportunities, int timeout, CancellationToken cancelToken)
	{
		var idleTimes = QueryIdleProcessorCycleTime ();
		await Task.Delay (timeout * (idleTimes == null ? 2 : 1));
		if (idleTimes == null) return;
		long startCycles = idleTimes.Sum();
		while (!cancelToken.IsCancellationRequested)
		{
			idleTimes = QueryIdleProcessorCycleTime ();
			if (idleTimes == null || idleTimes.Sum() - startCycles > computeOpportunities) return;
			await Task.Delay (100).ConfigureAwait (false);
		}
	}
	
	[DllImport ("Kernel32", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs (UnmanagedType.Bool)]
	static extern bool QueryIdleProcessorCycleTime (ref int byteCount, long [] cycleTimes);

	public static long [] QueryIdleProcessorCycleTime ()
	{
		if (Environment.OSVersion.Version.Major < 6) return null;
		var cycleTimes = new long [Environment.ProcessorCount];
		int len = Environment.ProcessorCount * 8;
		if (!QueryIdleProcessorCycleTime (ref len, cycleTimes)) return null;
		return cycleTimes;
	}
	#endregion

	#region Catch
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
	
	public async static Task<T> ToTaskOfT<T> (this Task t)
	{
		await t;
		return default(T);
	}
	
	public static Task<T> OnTimeout<T> (this Task t, T valueIfError)
	{
		return OnTimeout (ToTaskOfT<T> (t), valueIfError);
	}

	public static Task<T> OnTimeout<T> (this Task<T> t, T valueIfError)
	{
		return t.Catch<T,TimeoutException> (valueIfError);
	}

	public static Task<T> OnCancellation<T> (this Task t, T valueIfError)
	{
		return OnCancellation (ToTaskOfT<T> (t), valueIfError);
	}

	public static Task<T> OnCancellation<T> (this Task<T> t, T valueIfError)
	{
		return t.Catch<T,OperationCanceledException> (valueIfError);
	}
	#endregion
}

async void Main()
{
	var downloadTask = new WebClient().DownloadStringTaskAsync ("http://microsoft.com");
	string html = await downloadTask.WithTimeout (100);
	html.Dump();
}