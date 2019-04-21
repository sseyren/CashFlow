using System;
namespace CashFlow
{
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
