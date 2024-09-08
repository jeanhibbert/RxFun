<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

public static class Extensions
{
	// http://blogs.msdn.com/b/pfxteam/archive/2012/08/15/implementing-then-with-await.aspx
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

async void Main()
{	
}
