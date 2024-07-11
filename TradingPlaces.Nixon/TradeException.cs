using System.Runtime.Serialization;

namespace TradingPlaces.Nixon
{
	public sealed class TradeException : Exception
	{
		public string Ticker { get; }

		public TradeException(string ticker)
		{
			Ticker = ticker;
		}
	}
}