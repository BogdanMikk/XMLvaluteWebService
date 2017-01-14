using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.IO;
.



namespace MoiKostiliApp
{

    public class CurrencyRate
    {
        /// <summary>
        /// Закодированное строковое обозначение валюты
        /// Например: USD, EUR, AUD и т.д.
        /// </summary>
        public string CurrencyStringCode;

        /// <summary>
        /// Наименование валюты
        /// Например: Доллар, Евро и т.д.
        /// </summary>
        public string CurrencyName;

        /// <summary>
        /// Обменный курс
        /// </summary>
        public double ExchangeRate;
    }

    public class CurrencyRates
    {
        public class ValCurs
        {
            [XmlElementAttribute("Valute")]
            public ValCursValute[] ValuteList;
        }

        public class ValCursValute
        {

            [XmlElementAttribute("CharCode")]
            public string ValuteStringCode;

            [XmlElementAttribute("Name")]
            public string ValuteName;

            [XmlElementAttribute("Value")]
            public string ExchangeRate;
        }

        public static List<CurrencyRate> GetExchangeRates()
        {
            List<CurrencyRate> result = new List<CurrencyRate>();
            XmlSerializer xs = new XmlSerializer(typeof(ValCurs));
            XmlReader xr = new XmlTextReader(@"http://www.cbr.ru/scripts/XML_daily.asp"); //read XML
            foreach (ValCursValute valute in ((ValCurs)xs.Deserialize(xr)).ValuteList)
            {
                result.Add(new CurrencyRate()
                {
                    CurrencyName = valute.ValuteName,
                    CurrencyStringCode = valute.ValuteStringCode,
                    ExchangeRate = Convert.ToDouble(valute.ExchangeRate)
                });
            }
            return result;
        }

        class Program
        {
            static void Main(string[] args)
            {
                List<CurrencyRate> tmp = CurrencyRates.GetExchangeRates();
                //var USD = (tmp.FindLast(s => s.CurrencyStringCode == "USD").ExchangeRate.ToString());
                //var EUR = (tmp.FindLast(s => s.CurrencyStringCode == "EUR").ExchangeRate.ToString());
                //var UAH = (tmp.FindLast(s => s.CurrencyStringCode == "UAH").ExchangeRate.ToString());
                //Console.WriteLine(USD);
                //Console.WriteLine(EUR);
                //Console.WriteLine(UAH);
                //Console.ReadKey();
                string aa = "a";
                RBKServise.DailyInfo di = new RBKServise.DailyInfo();
                DateTime thisDay = DateTime.Today;
                System.DateTime On_date = thisDay;
                var Ds = di.GetCursOnDate(On_date);
                string sourceXml = Ds.GetXml(); //accept XML
                List<string> xmllist = new List<string>();
                XDocument ob = XDocument.Parse(sourceXml);
                var a = from x in ob.Descendants("ValuteCursOnDate")
                        where x.Descendants("VchCode").First().Value == "UAH"
                        select new
                        { 
                                Vname = x.Descendants("VchCode").First().Value,
                                Kurs = x.Descendants("Vcurs").First().Value
                                //UAH = x.Descendants("VchCode").First().Value == "UAH"
                        };
                foreach (var kur in a)
                {
                    aa = kur.Kurs;
                    //Console.WriteLine(aa);
                    //Console.WriteLine("{0}", kur.Kurs);
                }
                Console.WriteLine(aa);

                Console.ReadKey();
            }
        }
    }
}
