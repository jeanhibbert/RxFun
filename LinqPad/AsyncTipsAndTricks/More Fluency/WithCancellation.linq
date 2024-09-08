<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

public static class Extensions
{
	public static Task<TResult> WithCancellation<TResult> (this Task<TResult> task, CancellationToken cancelToken)
	{
		var tcs = new TaskCompletionSource<TResult> ();
		var reg = cancelToken.Register (() => tcs.TrySetCanceled ());
		task.ContinueWith (ant =>
		{
			reg.Dispose ();
			if (ant.IsCanceled)
				tcs.TrySetCanceled ();
			else if (ant.IsFaulted)
				tcs.TrySetException (ant.Exception.InnerException);
			else
				tcs.TrySetResult (ant.Result);
		});
		return tcs.Task;
	}	
}

void Main()
{
}