using cangqian.src.service;
using cangqian.src.tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cangqian
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        /***
         * 
         * 导入上回学生成绩excel文件
         * 
         * */
        private void button1_Click(object sender, EventArgs e)
        {
            //1、让用户选择xml文件路径
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = "d://";
            fileDialog1.Filter = "xlsx files (*.xlsx)|*.xlsx|xls files (*.xls)|*.xls|All files (*.*)|*.*";
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //2、渲染文件路径到text上面
            string filePath = fileDialog1.FileName; ;
            this.textBox1.Text = filePath;
            //3、读取excel内所有行记录，将结果渲染到dataTable内
            DataTable dt = Form1Service.loadExcelData(filePath);
            if (null == dt)
            {
                MessageBox.Show("加载XML失败,filepath=" + filePath);
                return;
            }
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.AutoGenerateColumns = true;
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }

        /***
         * 
         * 导入本次学生成绩excel文件
         * */
        private void button2_Click(object sender, EventArgs e)
        {
            //1、让用户选择xml文件路径
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = "d://";
            fileDialog1.Filter = "xlsx files (*.xlsx)|*.xlsx|xls files (*.xls)|*.xls|All files (*.*)|*.*";
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //2、读取excel内所有行记录，将结果渲染到dataTable内
            string filePath = fileDialog1.FileName;
            this.textBox2.Text = filePath;
            //3、读取excel内所有行记录，将结果渲染到dataTable内
            DataTable dt = Form1Service.loadExcelData(filePath);
            if (null == dt)
            {
                MessageBox.Show("加载XML失败,filepath=" + filePath);
                return;
            }
            this.dataGridView2.DataSource = dt;
            this.dataGridView2.AutoGenerateColumns = true;
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try {
                DataTable td1 = (DataTable)this.dataGridView1.DataSource;
                DataTable td2 = (DataTable)this.dataGridView2.DataSource;
                if (null == td1 || null == td2)
                {
                    MessageBox.Show("请导入excel数据！");
                    return;
                }
                DataRowCollection rows1 = td1.Rows;
                DataRowCollection rows2 = td2.Rows;
                if (rows1.Count == 0 || rows2.Count == 0)
                {
                    MessageBox.Show("请导入excel数据！");
                    return;
                }
                DataTable[] tables = Form2Service.compare(rows1, rows2);
                GridTools.renderTable(this.dataGridView3, tables[0]);
                GridTools.renderTable(this.dataGridView4, tables[1]);
                GridTools.renderTable(this.dataGridView5, tables[2]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.form1.Visible = true;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Program.form1.Visible = true;
        }
    }
}
