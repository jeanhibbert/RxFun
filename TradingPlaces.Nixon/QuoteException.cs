using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlaces.Nixon
{
	public sealed class QuoteException : Exception
	{
		public string Ticker { get; }

		public QuoteException(string ticker)
		{
			Ticker = ticker;
		}
	}
}
