<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

public static class Extensions
{
	public static Task ToTask (this WaitHandle waitHandle, int timeout)
	{
		return ToTask (waitHandle, CancellationToken.None, timeout);
	}

	public static Task ToTask (this WaitHandle waitHandle, CancellationToken cancelToken = default(CancellationToken), int timeout = -1)
	{
		var tcs = new TaskCompletionSource<object> ();

		RegisteredWaitHandle token = null;

		// There's a bug in RegisterWaitForSingleObject where calling token.Unregister on an AutoResetEvent can cause
		// subsequent invocations to trigger immediately. We'll leave it instead to the GC to do the job.

		var cancelTokenReg = cancelToken.Register (() => tcs.TrySetCanceled ());

		if (cancelToken.IsCancellationRequested) return tcs.Task;

		token = ThreadPool.RegisterWaitForSingleObject (
			waitHandle,
			(state, timedOut) =>
			{
				cancelTokenReg.Dispose ();
				if (timedOut) tcs.TrySetException (new TimeoutException ());
				else tcs.TrySetResult (null);
				GC.KeepAlive (token);
			},
			null,
			timeout,
			true);

		return tcs.Task;
	}
}

async void Main()
{
	var wh = new ManualResetEvent (false);
	Task.Delay(1000).ContinueWith (ant => wh.Set());
	"awaiting".Dump();
	await wh.ToTask();
	"Signaled".Dump();
}