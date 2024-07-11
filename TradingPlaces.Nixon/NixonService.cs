using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace TradingPlaces.Nixon
{
    public sealed class NixonService : INixonService
	{
		private const double FailureRate = 0.2;

		private static readonly Random Seed = new Random();

		private static readonly Regex TickerRegex = new Regex("^[A-Z0-9]{3,5}$", RegexOptions.Compiled);

		private readonly ConcurrentDictionary<string, decimal> _lastPrices = new ConcurrentDictionary<string, decimal>();

		/// <inheritdoc />
		public decimal GetQuote(string ticker)
		{
			if (!TickerRegex.IsMatch(ticker))
			{
				throw new ArgumentException("Invalid ticker.", "ticker");
			}
			if (ShouldFail(0.2))
			{
				throw new QuoteException(ticker);
			}
			return _lastPrices.AddOrUpdate(ticker, GetInitialQuote, UpdateQuote);
		}

		/// <inheritdoc />
		public decimal Buy(string ticker, int quantity)
		{
			return Trade(ticker, quantity);
		}

		/// <inheritdoc />
		public decimal Sell(string ticker, int quantity)
		{
			return Trade(ticker, quantity) * -1m;
		}

		private decimal Trade(string ticker, int quantity)
		{
			if (!TickerRegex.IsMatch(ticker))
			{
				throw new ArgumentException("Invalid ticker.", "ticker");
			}
			if (quantity <= 0)
			{
				throw new ArgumentOutOfRangeException("quantity", quantity, "Quantity must be greater than 0.");
			}
			if (ShouldFail(0.2))
			{
				throw new TradeException(ticker);
			}
			if (!_lastPrices.TryGetValue(ticker, out var value))
			{
				value = GetInitialQuote(ticker);
			}
			return value * (decimal)quantity;
		}

		private static decimal GetInitialQuote(string ticker)
		{
			return ToCorrectPrecision(Math.Pow(20.0, 2.0 + Seed.NextDouble()));
		}

		private static decimal UpdateQuote(string ticker, decimal originalQuote)
		{
			return GetNextPrice(originalQuote);
		}

		private static decimal GetNextPrice(decimal originalQuote)
		{
			return ToCorrectPrecision(originalQuote + GetDelta(originalQuote));
		}

		private static decimal GetDelta(decimal originalQuote)
		{
			return originalQuote * (decimal)((Seed.NextDouble() - 0.5) * 0.02);
		}

		private static decimal ToCorrectPrecision(double quote)
		{
			return ToCorrectPrecision((decimal)quote);
		}

		private static decimal ToCorrectPrecision(decimal quote)
		{
			return Math.Floor(quote * 100m) / 100m;
		}

		private static bool ShouldFail(double probability)
		{
			return Seed.NextDouble() < probability;
		}
	}

}
