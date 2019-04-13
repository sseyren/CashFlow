using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;

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

        public DateTime StartAt, EndAt;

        public Currencies Base = Currencies.USD;
        public List<Currencies> Symbols = new List<Currencies> { Currencies.TRY, Currencies.JPY, Currencies.GBP, Currencies.EUR };

        public Dictionary<Currencies, List<Node>> currencyDict;

        public CurrencyFetcher()
        {
            currencyDict = new Dictionary<Currencies, List<Node>>();
            foreach (Currencies currency in Symbols)
                currencyDict[currency] = new List<Node>();
        }

        public Dictionary<Currencies, List<Node>> Fetch()
        {
            string jsonString;

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("../../exchangeratesapi-cache/history2.json"))
                {
                    // Read the stream to a string, and write the string to the console.
                    jsonString = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                throw e;
            }

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);

            foreach (var date in jsonObject.rates)
            {
                DateTime dt = DateTime.ParseExact(date.Name, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                foreach (var value in date.First)
                {
                    Enum.TryParse(value.Name, out Currencies currency);
                    currencyDict[currency].Add(new Node { Time = dt, Value = value.First });
                    //Console.WriteLine(currencyDict[Currencies.TRY].Count);
                }
            }

            foreach (Currencies key in currencyDict.Keys)
            {
                currencyDict[key].Sort((x, y) => DateTime.Compare(x.Time, y.Time));
            }

            return currencyDict;
        }
    }
}
