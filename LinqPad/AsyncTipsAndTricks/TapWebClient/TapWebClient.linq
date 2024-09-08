<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Cache</Namespace>
</Query>

/* When it comes to downloading a web page, the bad news is that neither WebClient nor HttpClient
expose asynchronous functions that accept an IProgress<T> object.

The good news that it's pretty simple to write a wrapper around WebClient that offers the full TAP
experience - complete with progress reporting and cancellation! All we do is wrap the methods that
are there to support the old EAP (Event-based-Asynchronous Pattern), and voila, the TapWebClient!
And because it uses WebClient, it supports FTP, too. You're free to paste this class into your
applications and adapt it as appropriate.

Notice the that the IProgress<T> argument is not IProgress<int>, it's of type
IProgress<DownloadProgressChangedEventArgs>. The DownloadProgressChangedEventArgs
type gives more detailed progress reporting	than just percent complete.

Notice also that we're wrapping calls to the asynchronous methods in WebClient in Task.Run. This
is to work around a bug in the CLR whereby asynchronous network methods are only *partly*
asynchronous. Things like proxy and DNS resolution are actually done synchronously!
*/

async void Main()
{
	var progress = new Progress <DownloadProgressChangedEventArgs> (
		p => new { p.BytesReceived, p.ProgressPercentage }.ToString().Dump() );
	
	string html = await new TapWebClient().DownloadStringAsync (
		"http://www.albahari.com/ispell/allwords.txt",
		CancellationToken.None,
		progress);
		
	html.Length.Dump();
}

public class TapWebClient
{
	public string BaseAddress { get; set; }
	public RequestCachePolicy CachePolicy { get; set; }
	public bool UseDefaultCredentials { get; set; }
	public ICredentials Credentials { get; set; }	
	public WebHeaderCollection Headers { get; set; }
	public IWebProxy Proxy { get; set; }
	
	public TapWebClient()
	{
		var defaultClient = new WebClient();
		BaseAddress = defaultClient.BaseAddress;
		Headers = defaultClient.Headers;
		Proxy = defaultClient.Proxy;
	}
	
	public async Task<string> DownloadStringAsync (string uri,
		CancellationToken cancelToken = default (CancellationToken),
		IProgress<DownloadProgressChangedEventArgs> progress = null)
	{
		return await Task.Run (() => GetWebClient (cancelToken, progress).DownloadStringTaskAsync (uri)).ConfigureAwait(false);
	}
	
	public async Task<byte[]> DownloadDataAsync (string uri,
		CancellationToken cancelToken = default (CancellationToken),
		IProgress<DownloadProgressChangedEventArgs> progress = null)
	{
		return await Task.Run (() => GetWebClient (cancelToken, progress).DownloadDataTaskAsync (uri)).ConfigureAwait(false);
	}
	
	public async Task DownloadFileAsync (string uri, string fileName,
		CancellationToken cancelToken = default (CancellationToken),
		IProgress<DownloadProgressChangedEventArgs> progress = null)
	{
		await Task.Run (() => GetWebClient (cancelToken, progress).DownloadFileTaskAsync (uri, fileName)).ConfigureAwait(false);
	}
	
	WebClient GetWebClient (CancellationToken cancelToken, IProgress<DownloadProgressChangedEventArgs> progress)
	{
		var wc = new WebClient
		{
			BaseAddress = BaseAddress,
			CachePolicy = CachePolicy,
			UseDefaultCredentials = UseDefaultCredentials,
			Credentials = Credentials,
			Headers = Headers,
			Proxy = Proxy
		};
		
		if (cancelToken != CancellationToken.None) cancelToken.Register (() => wc.CancelAsync());			
		if (progress != null) wc.DownloadProgressChanged += (sender, args) => progress.Report (args);
			
		return wc;
	}
}