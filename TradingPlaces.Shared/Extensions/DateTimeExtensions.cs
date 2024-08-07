﻿using System;

namespace TradingPlaces.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToWeekday(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return date.AddDays(2);
                case DayOfWeek.Sunday:
                    return date.AddDays(1);
                default:
                    return date;
            }
        }
    }
}