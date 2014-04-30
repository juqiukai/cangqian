using cangqian.src.manager;
using cangqian.src.service;
using cangqian.src.tools;
using cangqian.src.vo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cangqian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /**
         * 选择Excel文件
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
            string filePath = fileDialog1.FileName;;
            this.textBox1.Text = filePath;
        }

        /**
         * 点击“导入”按钮
         * */
        private void button3_Click(object sender, EventArgs e)
        {
            try {
                //1、获取Excel文件路径
                string filePath = this.textBox1.Text;
                if ("".Equals(filePath))
                {
                    MessageBox.Show("先选择Excel文件");
                }

                //2、获取原始的excel文件并渲染
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

                //3、 获取校验失败的数据并汇总到一个新的datatable中
                DataTable dt2 = Form1Service.getCheckFailedDataTable(dt.Rows);
                this.dataGridView2.DataSource = dt2;
                this.dataGridView2.AutoGenerateColumns = true;
                foreach (DataGridViewRow row in this.dataGridView2.Rows)
                {
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        
        /**
         * 点击“统计”按钮
         * */
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable td = (DataTable)this.dataGridView1.DataSource;
                DataTable[] tables = Form1Service.calc(td.Rows);
                GridTools.renderTable(this.dataGridView_stat, tables[0]);
                GridTools.renderTable(this.dataGridView_high, tables[1]);
                GridTools.renderTable(this.dataGridView_low, tables[2]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("sdfsdfds");
            System.Console.WriteLine("sdsfdsfdsfdsfdsfds");
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Hide();   
            Form2 form2 = new Form2();
            form2.Show();
        }
        
    }
}
