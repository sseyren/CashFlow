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

namespace CashFlow
{
    public static class License
    {
        public const string MergedLicenses =
    @" ---- CashFlow
The MIT License (MIT)

Copyright (c) 2019 Serhat Seyren

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

 ---- OxyPlot
The MIT License (MIT)

Copyright (c) 2014 OxyPlot contributors

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the
""Software""), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

 ---- Newtonsoft.Json
The MIT License (MIT)

Copyright (c) 2007 James Newton-King

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the ""Software""), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
";
    }

    public enum Currencies
    {
        [StringValue("Avustralya Doları")] AUD,
        [StringValue("Bulgar Levi")] BGN,
        [StringValue("Brezilya Reali")] BRL,
        [StringValue("Kanada Doları")] CAD,
        [StringValue("İsviçre Frangı")] CHF,
        [StringValue("Çin Yuanı")] CNY,
        [StringValue("Çek Korunası")] CZK,
        [StringValue("Danimarka Kronu")] DKK,
        [StringValue("Euro")] EUR,
        [StringValue("İngiliz Sterlini")] GBP,
        [StringValue("Hong Kong Doları")] HKD,
        [StringValue("Hırvat Kunası")] HRK,
        [StringValue("Macar Forinti")] HUF,
        [StringValue("Endonezya Rupisi")] IDR,
        [StringValue("İsrail Şekeli")] ILS,
        [StringValue("Hint Rupisi")] INR,
        [StringValue("İzlanda Kronu")] ISK,
        [StringValue("Japon Yeni")] JPY,
        [StringValue("Güney Kore Wonu")] KRW,
        [StringValue("Meksika Pezosu")] MXN,
        [StringValue("Malezya Ringgiti")] MYR,
        [StringValue("Norveç Kronu")] NOK,
        [StringValue("Yeni Zellanda Doları")] NZD,
        [StringValue("Filipin Pezosu")] PHP,
        [StringValue("Polonya Zlotisi")] PLN,
        [StringValue("Rumen Leyi")] RON,
        [StringValue("Rus Rublesi")] RUB,
        [StringValue("İsveç Kronu")] SEK,
        [StringValue("Singapur Doları")] SGD,
        [StringValue("Tayland Bahtı")] THB,
        [StringValue("Türk Lirası")] TRY,
        [StringValue("Amerikan Doları")] USD,
        [StringValue("Güney Afrika Randı")] ZAR
    }

    public class Node
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }

    /*
     * Enumerate string nitelik ekleme konusunda Stefan Sedich'e teşekkürler,
     * bu kısmı doğrudan onun blog gönderisinden kopyaladım:    
     * https://weblogs.asp.net/stefansedich/enum-with-string-values-in-c
     */

    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
        #endregion
    }

    public static class EnumStringStatic
    {
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            System.Reflection.FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
}
