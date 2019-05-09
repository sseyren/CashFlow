#region License
// This file is part of CashFlow.
//
// The MIT License (MIT)
//
// Copyright (c) 2019 Serhat Seyren
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

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
        public List<Currencies> Symbols = new List<Currencies>
        {
            Currencies.CAD,
            Currencies.EUR,
            Currencies.ILS,
            Currencies.RON,
            Currencies.TRY
        };

        public Dictionary<Currencies, List<Node>> Data = new Dictionary<Currencies, List<Node>>();

        public List<Node> TopValues = new List<Node>();
        public List<Node> BottomValues = new List<Node>();
        public List<Node[]> TopChanges = new List<Node[]>();
        public List<Node[]> BottomChanges = new List<Node[]>();

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

            Data.Clear();
            foreach (Currencies currency in Symbols)
                Data[currency] = new List<Node>();

            foreach (var date in jsonObject.rates)
            {
                DateTime dt = DateTime.ParseExact(date.Name, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                foreach (var value in date.First)
                {
                    Enum.TryParse(value.Name, out Currencies currency);
                    Data[currency].Add(new Node { Time = dt, Value = value.First });
                }
            }

            foreach (Currencies key in Data.Keys)
                Data[key].Sort((x, y) => DateTime.Compare(x.Time, y.Time));

            ComputeAnnotations();

            return Data;
        }

        private void ComputeAnnotations()
        {
            TopValues.Clear();
            BottomValues.Clear();
            TopChanges.Clear();
            BottomChanges.Clear();

            foreach (KeyValuePair<Currencies, List<Node>> pair in Data)
            {
                if (pair.Value.Count < 3)
                    continue;

                Node top = new Node();
                Node bottom = new Node { Time = pair.Value[0].Time, Value = pair.Value[0].Value };

                Node[] topChange = new Node[2] { new Node(), new Node() };
                Node[] bottomChange = new Node[2]
                {
                    new Node { Time = pair.Value[0].Time, Value = pair.Value[0].Value },
                    new Node { Time = pair.Value[1].Time, Value = pair.Value[1].Value }
                };

                foreach (Node node in pair.Value)
                {
                    if (node.Value > top.Value)
                        top = node;
                    else if (node.Value < bottom.Value)
                        bottom = node;
                }

                for (int i = 1; i < pair.Value.Count; i++)
                {
                    if ((pair.Value[i].Value - pair.Value[i - 1].Value) > (topChange[1].Value - topChange[0].Value))
                        topChange = new Node[2] { pair.Value[i - 1], pair.Value[i] };
                    else if ((pair.Value[i].Value - pair.Value[i - 1].Value) < (bottomChange[1].Value - bottomChange[0].Value))
                        bottomChange = new Node[2] { pair.Value[i - 1], pair.Value[i] };
                }

                TopValues.Add(top);
                BottomValues.Add(bottom);
                TopChanges.Add(topChange);
                BottomChanges.Add(bottomChange);
            }
        }
    }
}
