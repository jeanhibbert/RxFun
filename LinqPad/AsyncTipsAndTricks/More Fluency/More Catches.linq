<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

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
	
	#region Simple error value
	public static Task<T> Catch<T,TError> (this Task<T> task, T onError) where TError : Exception
	{
		return task.Catch<T,TError> (ex => onError);
	}
	#endregion
	
	#region Simple Task	
	public static Task<T> Catch<T,TError> (this Task task, T onError) where TError : Exception
	{
		return task.ToTaskOfT<T>().Catch<T,TError> (onError);
	}
	#endregion
	
	#region ToTaskOfT
	public async static Task<T> ToTaskOfT<T> (this Task t)
	{
		await t;
		return default(T);
	}
	#endregion
	
	#region ToTaskOfT - optimized
	public static Task<T> ToTaskOfT_<T> (this Task t)
	{
		if (t is Task<T>) return (Task<T>)t;
		var tcs = new TaskCompletionSource<T> ();
		t.ContinueWith (ant =>
		{
			if (ant.IsCanceled) tcs.SetCanceled ();
			else if (ant.IsFaulted) tcs.SetException (ant.Exception.InnerException);
			else tcs.SetResult (default (T));
		});
		return tcs.Task;
	}
	#endregion
	
	#region Exceptions to tasks
	public static Task ToTask (this Exception ex)
	{
		var tcs = new TaskCompletionSource<object> ();
		tcs.SetException (ex);
		return tcs.Task;
	}

	public static Task<T> ToTask<T> (this Exception ex)
	{
		var tcs = new TaskCompletionSource<T>();
		tcs.SetException (ex);
		return tcs.Task;
	}
	#endregion
	
	#region Parameterless
	public static Task<T> Catch<T> (this Task<T> task, T valueIfError = default(T))
	{
		return task.Catch<T,Exception> (valueIfError);
	}
	
	public static Task Catch (this Task task)
	{
		return task.Catch<object,Exception> (null);
	}	
	#endregion
	
	#region Timeouts and cancellation
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

#region Demo
async Task<int> SomeAsyncMethod()
{
	await Task.Delay(1000);
	return 123;
}

async void Main()
{
	int result = await SomeAsyncMethod().OnTimeout(0).OnCancellation(-1);
	
	int? result2 = await SomeAsyncMethod().OnTimeout<int?>(null).OnCancellation<int?>(null);
}
#endregion