using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel; 
namespace ExportExcel
{
    public class ReadFile
    {
        public void Leer()
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            string str = string.Empty, str1 = string.Empty;
            int rCnt = 0;
            int cCnt = 0;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(@"c:\CELINA\listado.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            List<string> lista=new List<string>();
            for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
            {
                    str = Convert.ToString((range.Cells[rCnt,1] as Excel.Range).Value2).Trim();
                    str1 = Convert.ToString((range.Cells[rCnt, 2] as Excel.Range).Value2).Trim();
                
                lista.Add(string.Format("insert Producto(Descripcion,Codigo,Posicion_ArancelariaId) values ('{0}','{1}',11)", str.Trim(), str1.Trim()));
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();
            guardarArchivo(lista);
            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        private void guardarArchivo(List<string> lista)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"insert.sql"))
            {
                foreach (string line in lista)
                {
                        file.WriteLine(line);
                }
            }

        }

        
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        } 
    }
}
