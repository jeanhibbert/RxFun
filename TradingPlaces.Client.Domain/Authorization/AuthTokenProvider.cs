using TradingPlaces.Shared;

namespace TradingPlaces.Client.Domain.Authorization
{
    public class AuthTokenProvider : IAuthTokenProvider
    {
        public AuthTokenProvider(string authToken)
        {
            AuthToken = authToken;
        }

        public string AuthToken { get; private set; }
    }
}