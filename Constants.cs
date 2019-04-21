using System;
namespace CashFlow
{
    public enum Currencies
    {
        AUD, BGN, BRL, CAD, CHF, CNY, CZK, DKK, EUR, GBP, HKD, HRK, HUF, IDR, ILS, INR, ISK, JPY, KRW, MXN, MYR, NOK, NZD, PHP, PLN, RON, RUB, SEK, SGD, THB, TRY, USD, ZAR
    }

    public class Node
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }
}
