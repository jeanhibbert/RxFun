﻿using System;
using Newtonsoft.Json.Serialization;

namespace TradingPlaces.Shared.DTO.Analytics
{
    public class HistoricPositionDto
    {

        public DateTimeOffset Timestamp { get; set; }
        public decimal UsdPnl { get; set; }

        public override string ToString()
        {
            return string.Format("Timestamp: {0}, UsdPnl: {1}", Timestamp, UsdPnl);
        }
    }
}