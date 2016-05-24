using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeerDolar
{
    public class GeneradorXRates:GeneradorBase
    {
        public void GeneradoXRates(DateTime  date)
        {
            Page = "http://www.x-rates.com/historical/?from=ARS&amount=1&date=" + date.ToString("yyyy") + "-" +
                   date.ToString("MM") + "-" + date.ToString("dd");
            GeneradorRepor(date);   
        }

        private Dictionary<DateTime, double> Dolar = new Dictionary<DateTime, double>();

        protected override void ExtractValuePerDay( string html,DateTime date)
        {
            string tag="<a href='/graph/?from=USD&amp;to=ARS'>";
            var startTag = tag ;
            int startIndex = html.IndexOf(startTag, StringComparison.Ordinal) + startTag.Length;
            int endIndex = html.IndexOf("</a>", startIndex, StringComparison.Ordinal);
            double dolar= Convert.ToDouble(html.Substring(startIndex, endIndex - startIndex).Replace(".",","));
            Dolar.Add(date,dolar);
            DateDolar=Dolar;
        }
    }
}
