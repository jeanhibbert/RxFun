using System;
using System.Collections.Generic;
using TradingPlaces.Shared.DTO.ReferenceData;

namespace TradingPlaces.Client.Domain.ServiceClients
{
    internal interface IReferenceDataServiceClient
    {
        IObservable<IEnumerable<CurrencyPairUpdateDto>> GetCurrencyPairUpdatesStream();
    }
}