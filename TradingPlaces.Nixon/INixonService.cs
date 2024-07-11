namespace TradingPlaces.Nixon
{
	public interface INixonService
	{
		/// <summary>
		/// Returns the current price for the specified ticker.
		/// </summary>
		/// <param name="ticker">Ticker for which to retrieve current price.</param>
		/// <exception cref="T:Nixon.QuoteException">Thrown is retrieval of quote fails.</exception>
		/// <returns>Current price in USD.</returns>
		decimal GetQuote(string ticker);

		/// <summary>
		/// Executes a buy trade for the specified ticker of the specified size, and returns the total value of the trade (which should be +ve).
		/// </summary>
		/// <param name="ticker">Ticker for which to execute trade.</param>
		/// <param name="quantity">Size of trade.</param>
		/// <exception cref="T:Nixon.TradeException">Thrown is trade fails to execute correctly.</exception>
		/// <returns>Total value of trade in USD.</returns>
		decimal Buy(string ticker, int quantity);

		/// <summary>
		/// Executes a sell trade for the specified ticker of the specified size, and returns the total value of the trade (which should be -ve).
		/// </summary>
		/// <param name="ticker">Ticker for which to execute trade.</param>
		/// <param name="quantity">Size of trade.</param>
		/// <exception cref="T:Nixon.TradeException">Thrown is trade fails to execute correctly.</exception>
		/// <returns>Total value of trade in USD.</returns>
		decimal Sell(string ticker, int quantity);
	}
}