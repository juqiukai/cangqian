using cangqian.src.vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.manager
{
    class DataRowManager
    {
        public static List<DataRow> findAllRightRows(DataRowCollection rows, List<CheckCellCondition> list)
        {
            if (null == rows || rows.Count == 0) return null;
            List<DataRow> dataRowList = new List<DataRow>();
            foreach (DataRow row in rows)
            {
                if (checkRow(row, list))
                {
                    dataRowList.Add(row);
                }
            }
            return dataRowList;
        }

        public static List<DataRow> findAllErrorRows(DataRowCollection rows, List<CheckCellCondition> list)
        {
            if (null == rows || rows.Count == 0) return null;
            List<DataRow> dataRowList = new List<DataRow>();
            foreach (DataRow row in rows)
            {
                if (!checkRow(row, list))
                {
                    dataRowList.Add(row);
                }
            }
            return dataRowList;
        }

        private static bool checkRow(DataRow dataRow, List<CheckCellCondition> list)
        {
            foreach (CheckCellCondition condition in list)
            {
                if (!condition.check(dataRow).isSuccess())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
