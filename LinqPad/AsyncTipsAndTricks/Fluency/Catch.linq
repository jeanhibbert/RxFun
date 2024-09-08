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
	
	#region Typed
	public static Task<string> CatchWebError (this Task<string> task)
	{
		return task.Catch<string,WebException> (ex => "Web Error " + ex.Status);
	}
	#endregion
}

void Main() { GetPage3().Dump(); }

#region Before
async Task<string> GetPage1()
{
	try
	{
		return await new WebClient().DownloadStringTaskAsync ("http://asdfqwergfsd");	
	}
	catch (WebException ex)
	{
		return "Web Error " + ex.Status;
	}	
}
#endregion

#region After
Task<string> GetPage2()
{
	return new WebClient().DownloadStringTaskAsync ("http://asdfqwergfsd")
		.Catch<string,WebException> (ex => "Web Error " + ex.Status);
}
#endregion

#region Typed
Task<string> GetPage3()
{
	return new WebClient().DownloadStringTaskAsync ("http://asdfqwergfsd").CatchWebError();
}
#endregion