using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.tools
{
    class ExcelTools
    {
        public static ArrayList dumpXml2ArrayList(string filePath)
        {
            return dumpXml2ArrayList(filePath, 1, 1);
        }

        public static ArrayList dumpXml2ArrayList(string filePath, int beginRow, int beginCol)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Application.Workbooks.Add(filePath);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int colCount = usedRange.Columns.Count;

                if (beginRow <= 1 || beginRow > rowCount)
                {
                    beginRow = 1;
                }
                if (beginCol <= 1 || beginCol > colCount)
                {
                    beginCol = 1;
                }

                ArrayList list = new ArrayList();
                for (int a = beginRow; a <= rowCount; a++)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int b = beginCol; b <= colCount; b++)
                    {
                        string t = usedRange.Cells[a, b].Text;
                        sb.Append(t).Append("#$@");
                    }
                    System.Console.WriteLine(sb.ToString(0, sb.Length - 1));
                    list.Add(sb.ToString(0, sb.Length - 1));
                }
                return list;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }
        }

        public static DataTable dumpXml2DataTable(string filePath, int beginRow, int beginCol,string[] colNameArr)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Application.Workbooks.Add(filePath);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int colCount = usedRange.Columns.Count;

                if (beginRow <= 1 || beginRow > rowCount)
                {
                    beginRow = 1;
                }
                if (beginCol <= 1 || beginCol > colCount)
                {
                    beginCol = 1;
                }

                DataTable dt = new DataTable();
                foreach(string colName in colNameArr) {
                    dt.Columns.Add(colName);
                }


                for (int a = beginRow; a <= rowCount; a++)
                {
                    DataRow dr = dt.NewRow();
                    for (int b = beginCol; b <= colCount; b++)
                    {
                        dr[b-beginCol] = ((string)(usedRange.Cells[a, b].Text)).Trim();
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return null;
        }

        public static DataTable dumpXml2DataTable(string filePath, string[] colNameArr)
        {
            return dumpXml2DataTable(filePath, 1, 1, colNameArr);
        }
    }

    class BaseResult {
        private int code = 200;
        private object data;
        private string msg = "SUC";

        public BaseResult(int code,object data,string msg)
        {
            this.code = code;
            this.data = data;
            this.msg = msg;
        }

        public BaseResult(object data)
        {
            this.data = data;
        }
    }

}
