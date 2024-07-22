using TradingPlaces.Client.Concurrency;
using TradingPlaces.Client.Domain;
using TradingPlaces.Client.Domain.Instrumentation;
using TradingPlaces.Client.UI.Blotter;
using TradingPlaces.Client.UI.Connectivity;
using TradingPlaces.Client.UI.Shell;
using TradingPlaces.Client.UI.SpotTiles;
using TradingPlaces.Shared.Logging;
using Autofac;

namespace TradingPlaces.Client
{
    public abstract class BootstrapperBase
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Domain.ReactiveTrader>().As<IReactiveTrader>().SingleInstance();
            builder.RegisterType<DebugLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterType<NullProcessorMonitor>().As<IProcessorMonitor>().SingleInstance();
            builder.RegisterType<ConstantRatePump>().As<IConstantRatePump>();

            builder.RegisterType<ShellViewModel>().As<IShellViewModel>().ExternallyOwned();
            builder.RegisterType<SpotTilesViewModel>().As<ISpotTilesViewModel>().ExternallyOwned();
            builder.RegisterType<SpotTileViewModel>().As<ISpotTileViewModel>().ExternallyOwned();
            builder.RegisterType<SpotTileErrorViewModel>().As<ISpotTileErrorViewModel>().ExternallyOwned();
            builder.RegisterType<SpotTileConfigViewModel>().As<ISpotTileConfigViewModel>().ExternallyOwned();
            builder.RegisterType<SpotTilePricingViewModel>().As<ISpotTilePricingViewModel>().ExternallyOwned();
            builder.RegisterType<OneWayPriceViewModel>().As<IOneWayPriceViewModel>().ExternallyOwned();
            builder.RegisterType<SpotTileAffirmationViewModel>().As<ISpotTileAffirmationViewModel>().ExternallyOwned();
            builder.RegisterType<BlotterViewModel>().As<IBlotterViewModel>().ExternallyOwned();
            builder.RegisterType<TradeViewModel>().As<ITradeViewModel>().ExternallyOwned();
            builder.RegisterType<ConnectivityStatusViewModel>().As<IConnectivityStatusViewModel>().ExternallyOwned();

            RegisterTypes(builder);

            return builder.Build();
        }

        protected abstract void RegisterTypes(ContainerBuilder builder);
    }
}
