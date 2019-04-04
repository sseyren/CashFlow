using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

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

    public class CurrencyFetcher
    {
        public const string BaseURL = "https://api.exchangeratesapi.io/history";

        public DateTime StartAt = DateTime.Today;
        public DateTime EndAt = DateTime.Today - new TimeSpan(8, 0, 0, 0);

        public Currencies Base = Currencies.TRY;
        public List<Currencies> Symbols;

        public void Fetch()
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("../../exchangeratesapi-cache/history1.json"))
                {
                    // Read the stream to a string, and write the string to the console.
                    string jsonString = sr.ReadToEnd();
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
                    foreach (var date in jsonObject.rates)
                    {
                        Console.WriteLine(date.Name);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
