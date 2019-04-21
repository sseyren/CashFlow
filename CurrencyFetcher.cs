using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace CashFlow
{
    public class DateException : Exception
    {
        public override string Message => "Geçerli bir tarih aralığı girilmeli.";
    }

    public class SymbolsException : Exception
    {
        public override string Message => "Gösterilecek hiç kur yok.";
    }

    public class CurrencyFetcher
    {
        public const string BaseURL = "https://api.exchangeratesapi.io/history";

        public DateTime StartAt, EndAt;

        public Currencies Base = Currencies.USD;
        public List<Currencies> Symbols = new List<Currencies> { Currencies.TRY, Currencies.JPY, Currencies.GBP, Currencies.EUR };

        public Dictionary<Currencies, List<Node>> currencyDict = new Dictionary<Currencies, List<Node>>();

        public CurrencyFetcher()
        {
            EndAt = DateTime.Today;
            StartAt = EndAt.AddYears(-1);
        }

        private string RequestURL
        {
            get
            {
                const string dateStringFormat = "yyyy-MM-dd";
                string symbols = string.Empty;

                foreach (Currencies symbol in Symbols)
                    symbols += symbol.ToString() + ",";

                return BaseURL + $"?start_at={StartAt.ToString(dateStringFormat)}&end_at={EndAt.ToString(dateStringFormat)}" +
                    $"&symbols={symbols.Substring(0, symbols.Length - 1)}&base={Base.ToString()}";
            }
        }

        public Dictionary<Currencies, List<Node>> Fetch()
        {
            if (StartAt >= EndAt)
                throw new DateException();
            if (Symbols.Count == 0)
                throw new SymbolsException();

            WebClient client = new WebClient(); 
            string jsonString = client.DownloadString(RequestURL);

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);

            currencyDict.Clear();
            foreach (Currencies currency in Symbols)
                currencyDict[currency] = new List<Node>();

            foreach (var date in jsonObject.rates)
            {
                DateTime dt = DateTime.ParseExact(date.Name, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                foreach (var value in date.First)
                {
                    Enum.TryParse(value.Name, out Currencies currency);
                    currencyDict[currency].Add(new Node { Time = dt, Value = value.First });
                }
            }

            foreach (Currencies key in currencyDict.Keys)
                currencyDict[key].Sort((x, y) => DateTime.Compare(x.Time, y.Time));

            return currencyDict;
        }
    }
}
