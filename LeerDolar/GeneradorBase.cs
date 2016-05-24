using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace LeerDolar
{
    public abstract class GeneradorBase
    {
        public string ConectUrl(DateTime date)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Page);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            return result;
        }

        protected string Page { get; set; }
        protected Dictionary<DateTime, double> _DateDolar;

      
        public Dictionary<DateTime, double> DateDolar
        {
            get
            {
                return this._DateDolar ?? new Dictionary<DateTime, double>();
            }
            set
            {
                this._DateDolar = value;
            }
        }

        public Dictionary<DateTime, double> GeneradorRepor(DateTime date)
        {
            var html=ConectUrl(date);
            ExtractValuePerDay(html,date);
            return DateDolar;
        }

        public void SaveData()
        {
            throw new NotImplementedException();
        }


        protected abstract void ExtractValuePerDay(string html,DateTime date);
    }
}
