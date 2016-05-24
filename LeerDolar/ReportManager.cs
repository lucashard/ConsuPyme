using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LeerDolar
{
    public class ReportManager
    {
        public Dictionary<DateTime, double> generar(DateTime date)
        {
            
            var manager = new GeneradorXRates();
            while (!manager.DateDolar.Any())
            {

                
                try
                {
                    manager.GeneradoXRates(date);

                }
                catch (Exception)
                {
                    Thread.Sleep(1000);    
                }
            }
            return manager.DateDolar;
        }
            

        }
    }

