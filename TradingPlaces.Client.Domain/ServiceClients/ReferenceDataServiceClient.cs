using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TradingPlaces.Client.Domain.Transport;
using TradingPlaces.Shared;
using TradingPlaces.Shared.DTO.ReferenceData;
using TradingPlaces.Shared.Extensions;
using TradingPlaces.Shared.Logging;
using Microsoft.AspNet.SignalR.Client;

namespace TradingPlaces.Client.Domain.ServiceClients
{
    class ReferenceDataServiceClient : ServiceClientBase, IReferenceDataServiceClient
    {
        private readonly ILog _log;

        public ReferenceDataServiceClient(IConnectionProvider connectionProvider, ILoggerFactory loggerFactory) : base(connectionProvider)
        {
            _log = loggerFactory.Create(typeof (ReferenceDataServiceClient));
        }

        public IObservable<IEnumerable<CurrencyPairUpdateDto>> GetCurrencyPairUpdatesStream()
        {
            return GetResilientStream(connection => GetCurrencyPairsForConnection(connection.ReferenceDataHubProxy), TimeSpan.FromSeconds(5));
        }

        private IObservable<IEnumerable<CurrencyPairUpdateDto>> GetCurrencyPairsForConnection(IHubProxy referenceDataHubProxy)
        {
            return Observable.Create<IEnumerable<CurrencyPairUpdateDto>>(observer =>
            {
                // subscribe to currency pair update feed first, otherwise there is a race condition 
                var currencyPairUpdateSubscription = referenceDataHubProxy.On<CurrencyPairUpdateDto>(ServiceConstants.Client.OnCurrencyPairUpdate,
                    dto => observer.OnNext(new[] {dto}));

                _log.Info("Sending currency pair subscription...");
                var sendSubscriptionDisposable = GetCurrencyPairUpdatesForConnection(referenceDataHubProxy)
                    .Subscribe(
                        currencyPairs =>
                        {
                            var currencyPairUpdateDtos = currencyPairs as CurrencyPairUpdateDto[] ?? currencyPairs.ToArray();
                            observer.OnNext(currencyPairUpdateDtos);

                            _log.InfoFormat("Subscribed to currency pairs and received {0} currency pairs.", currencyPairUpdateDtos.Count());
                        },
                        observer.OnError);

                var unsubscriptionDisposable = Disposable.Create(() =>
                {
                    // TODO we should add an unsubscription method server side (not a big deal as it gets cleaned-up by signalR
                });
                return new CompositeDisposable { currencyPairUpdateSubscription, unsubscriptionDisposable, sendSubscriptionDisposable };
            })
                .Publish()
                .RefCount();
        }


        private static IObservable<IEnumerable<CurrencyPairUpdateDto>> GetCurrencyPairUpdatesForConnection(IHubProxy referenceDataHubProxy)
        {
            return Observable.FromAsync(
                () => referenceDataHubProxy.Invoke<IEnumerable<CurrencyPairUpdateDto>>(ServiceConstants.Server.GetCurrencyPairs))
                .CacheFirstResult();
        }
    }
}