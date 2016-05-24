using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeerDolar
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DateTime date = new DateTime(2015, 01, 01, 23, 59, 59);
            new ReportManager().generar(date);
        }
    }
}