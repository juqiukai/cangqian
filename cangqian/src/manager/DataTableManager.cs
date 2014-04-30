using cangqian.src.tools;
using cangqian.src.vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.manager
{
    class DataTableManager
    {
        public static DataTable newDataTable(string[] columns)
        {
            DataTable newDt = new DataTable();
            foreach (string column in columns)
            {
                newDt.Columns.Add(column);
            }
            return newDt;
        }

        public static DataTable newDataTable(DataColumnCollection columns, List<DataRow> rows) 
        {
            DataTable newDt = new DataTable();
            foreach (DataColumn column in columns)
            {
                newDt.Columns.Add(column.ColumnName);
            }
            foreach(DataRow row in rows) {
                copyRow(newDt,row);
            }
            return newDt;
        }

        public static DataTable newDataTable(string[] columnNames, List<DataRow> rows)
        {
            DataTable newDt = new DataTable();
            foreach (string column in columnNames)
            {
                newDt.Columns.Add(column);
            }
            foreach (DataRow row in rows)
            {
                copyRow(newDt, row);
            }
            return newDt;
        }

        public static void copyRow(DataTable dt, DataRow dataRow)
        {
            DataRow dr = dt.NewRow();
            int colCount = dt.Columns.Count;
            for (int i = 0; i < colCount; i++)
            {
                dr[i] = dataRow[i];
            }
            dt.Rows.Add(dr);
        }
    }
}
