namespace TradingPlaces.Client.Configuration
{
    public interface IConfigurationProvider
    {
        string[] Servers { get; }
    }
}