using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QualityManager
{
    public partial class Form1 : Form
    {
        public int m { set; get; }
        public int n { set; get; }
        public double[] system_a2 { set; get; }
        public double[] system_d2 { set; get; }
        public double[] system_d3 { set; get; }
        public double[] system_d4 { set; get; }
        public double[] a2 { set; get; }
        public double[] d2 { set; get; }
        public double[] d3 { set; get; }
        public double[] d4 { set; get; }
        public double[,] transmit { set; get; }
        public double[] x { set; get; }
        public double[] r { set; get; }
        public double x_average { set; get; }
        public double r_average { set; get; }
        public double x_cl { set; get; }
        public double x_ucl { set; get; }
        public double x_lcl { set; get; }
        public double r_cl { set; get; }
        public double r_ucl { set; get; }
        public double r_lcl { set; get; }
        public double text_x { set; get; }
        public int select_m { set; get; }
        public int select_n { set; get; }
        public int system_status { set; get; }//0刚启动 1确认了样本规模
        //public DataGridViewTextBoxColumn[] column ;

        public Form1()
        {
            InitializeComponent();
            InitData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void InitData()
        {
            m = 20;
            n = 5;
            a2 = new double[100];
            d2 = new double[100];
            d3 = new double[100];
            d4 = new double[100];
            system_a2 = new double[100];
            system_d2 = new double[100];
            system_d3 = new double[100];
            system_d4 = new double[100];
            //column = new DataGridViewTextBoxColumn[100];

            for (int i = 0; i < 100; i++)
            {
                a2[i] = system_a2[i] = 0;
                d2[i] = system_d2[i] = 0;
                d3[i] = system_d3[i] = 0;
                d4[i] = system_d4[i] = 0;
            }
            a2[1] = system_a2[1] = 1.880;
            a2[2] = system_a2[2] = 1.023;
            a2[3] = system_a2[3] = 0.729;
            a2[4] = system_a2[4] = 0.577;
            a2[5] = system_a2[5] = 0.483;
            a2[6] = system_a2[6] = 0.419;
            a2[7] = system_a2[7] = 0.373;
            a2[8] = system_a2[8] = 0.337;
            a2[9] = system_a2[9] = 0.308;

            d2[1] = system_d2[1] = 1.128;
            d2[2] = system_d2[2] = 1.693;
            d2[3] = system_d2[3] = 2.059;
            d2[4] = system_d2[4] = 2.326;
            d2[5] = system_d2[5] = 2.534;
            d2[6] = system_d2[6] = 2.704;
            d2[7] = system_d2[7] = 2.847;
            d2[8] = system_d2[8] = 2.970;
            d2[9] = system_d2[9] = 3.078;

            d3[6] = system_d3[6] = 0.076;
            d3[7] = system_d3[7] = 0.136;
            d3[8] = system_d3[8] = 0.184;
            d3[9] = system_d3[9] = 0.223;

            d4[1] = system_d4[1] = 3.288;
            d4[2] = system_d4[2] = 2.574;
            d4[3] = system_d4[3] = 2.282;
            d4[4] = system_d4[4] = 2.114;
            d4[5] = system_d4[5] = 2.004;
            d4[6] = system_d4[6] = 1.924;
            d4[7] = system_d4[7] = 1.864;
            d4[8] = system_d4[8] = 1.816;
            d4[9] = system_d4[9] = 1.777;

            transmit = new double[m, n];
            x = new double[m];
            r = new double[m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    transmit[i, j] = 0;
                }
            }
            for (int i = 0; i < m; i++)
            {
                x[i] = r[i] = 0;
            }

            x_average = 0;
            r_average = 0;
            x_cl = 0;
            x_ucl = 0;
            x_lcl = 0;
            r_cl = 0;
            r_ucl = 0;
            r_lcl = 0;

            select_m = 0;
            select_n = 0;

            system_status = 0;
        }

        private void ResetToZero()
        {
            transmit = new double[m, n];
            x = new double[m];
            r = new double[m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    transmit[i, j] = 0;
                }
            }
            for (int i = 0; i < m; i++)
            {
                x[i] = r[i] = 0;
            }

            x_average = 0;
            r_average = 0;
            x_cl = 0;
            x_ucl = 0;
            x_lcl = 0;
            r_cl = 0;
            r_ucl = 0;
            r_lcl = 0;

            select_m = 0;
            select_n = 0;
        }

        private bool Calculate()
        {
            double max = 0;
            double min = 0;
            double sum_x = 0;
            double sum_r = 0;
            for (int i = 0; i < m; i++)
            {
                sum_x = 0;
                max = min = transmit[i, 0];
                for (int j = 0; j < n; j++)
                {
                    sum_x = sum_x + transmit[i, j];
                    if (transmit[i, j] > max)
                    {
                        max = transmit[i, j];
                    }
                    if (transmit[i, j] < min)
                    {
                        min = transmit[i, j];
                    }
                }
                x[i] = sum_x / Convert.ToDouble(n);
                r[i] = max - min;
            }
            sum_x = 0;
            sum_r = 0;
            for (int i = 0; i < m; i++)
            {
                sum_x = sum_x + x[i];
                sum_r = sum_r + r[i];
            }
            x_average = sum_x / Convert.ToDouble(m);
            r_average = sum_r / Convert.ToDouble(m);

            x_cl = x_average;
            if (a2[n - 1] * r_average > 0)
            {
                x_ucl = x_average + a2[n - 1] * r_average;
                x_lcl = x_average - a2[n - 1] * r_average;
            }
            else
            {
                x_ucl = x_average - a2[n - 1] * r_average;
                x_lcl = x_average + a2[n - 1] * r_average;
            }
            r_cl = r_average;
            if (d4[n - 1] * r_average >= d3[n - 1] * r_average)
            {
                r_ucl = d4[n - 1] * r_average;
                r_lcl = d3[n - 1] * r_average;
            }
            else
            {
                r_ucl = d3[n - 1] * r_average;
                r_lcl = d4[n - 1] * r_average;
            }

            for (int i = 0; i < m; i++)
            {
                if (x[i] > x_ucl || x[i] < x_lcl
                    || r[i] > r_ucl || r[i] < r_lcl)
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadSystemPara()
        {
            a2[n - 1] = system_a2[n - 1];
            d2[n - 1] = system_d2[n - 1];
            d3[n - 1] = system_d3[n - 1];
            d4[n - 1] = system_d4[n - 1];
            show_a2.Text = system_a2[n - 1].ToString();
            show_d2.Text = system_d2[n - 1].ToString();
            show_d3.Text = system_d3[n - 1].ToString();
            show_d4.Text = system_d4[n - 1].ToString();
        }

        private void UpadateListView()
        {
            dataGridViewX.RowCount = m * n;

            double h = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    dataGridViewX.Rows[i].Cells[j].Value = h;
        }

        private void show_n_ValueChanged(object sender, EventArgs e)
        {
            n = input_n.Value;
            LoadSystemPara();
        }

        private void show_m_ValueChanged(object sender, EventArgs e)
        {
            m = input_m.Value;
        }

        private void button_confirmmn_Click(object sender, EventArgs e)
        {  
            if (system_status == 0)
            {
                ResetToZero();
                UpadateListView();
                dataGridViewX.ReadOnly = false;
                button_saveandcreate.Enabled = true;
                input_m.Enabled = false;
                input_n.Enabled = false;
                system_status = 1;
                button_confire.Text = "重置";
            }
            else if (system_status == 1)
            {
                input_m.Enabled = true;
                input_n.Enabled = true;
                
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        dataGridViewX.Rows[i].Cells[j].Value = "";

                dataGridViewX.ReadOnly = true;
                button_saveandcreate.Enabled = false;
                button_confire.Text = "确定";
                system_status = 0;
            }
        }

        private void button_saveandcreate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    transmit[i, j] = Convert.ToDouble(dataGridViewX[i, j].Value);
                }
            }

            Form2 f2 = new Form2(this);
            f2.Show();
            //if (Calculate() == true)
            //{
            //    MessageBox.Show(this, "生产线处于可控状态！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    dataGridViewX.ReadOnly = true;
            //}
            //else
            //{
            //    MessageBox.Show(this, "生产线处于失控状态，请寻找系统性原因！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
    
        }


        private void DataGridViewRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridViewX.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, "n = "+(e.RowIndex + 1).ToString(),
                dataGridViewX.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridViewX.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        
        private void show_datalist_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (e.FormattedValue.ToString().Trim() != "")
                {
                    try
                    {
                        Double.Parse(e.FormattedValue.ToString());
                    }
                    catch (Exception ex)
                    {
                        e.Cancel = true;
                        MessageBox.Show(this, "请您输入整数或小数！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
