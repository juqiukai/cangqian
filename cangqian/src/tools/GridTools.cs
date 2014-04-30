using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cangqian.src.tools
{
    class GridTools
    {
        public static void renderTable(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
            dataGridView.AutoGenerateColumns = true;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }

    }
}
