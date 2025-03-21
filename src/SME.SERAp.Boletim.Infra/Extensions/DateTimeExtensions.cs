﻿namespace SME.SERAp.Boletim.Infra.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime InicioMes(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, 1, 0, 0, 0);
        }

        public static DateTime FinalMes(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month), 23, 59, 59);
        }
    }
}
