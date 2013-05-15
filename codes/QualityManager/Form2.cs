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
    public partial class Form2 : Form
    {
        private Form1 my_Form;
        public int m { set; get; }
        public int n { set; get; }
        public double[,] sample { set; get; }
        public double a2{ set; get; }
        public double d2{ set; get; }
        public double d3{ set; get; }
        public double d4{ set; get; }
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

        public Form2(Form1 myForm)
        {
            InitializeComponent();
            this.my_Form = myForm;
            initData();
            if (Calculate() == true)
            {
                MessageBox.Show(this, "生产线处于可控状态！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "生产线处于失控状态，请寻找系统性原因！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ShowDataResult();
            ShowChart();
        }

        public void initData ()
        {   
            m = my_Form.m;
            n = my_Form.n;

            sample = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sample[i, j] = my_Form.transmit[i, j];
                }
            }
            
            a2 = my_Form.a2[n - 1];
            d2 = my_Form.d2[n - 1];
            d3 = my_Form.d3[n - 1];
            d4 = my_Form.d4[n - 1];

            x = new double[m];
            r = new double[m];

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
                max = min = sample[i, 0];
                for (int j = 0; j < n; j++)
                {
                    sum_x = sum_x + sample[i, j];
                    if (sample[i, j] > max)
                    {
                        max = sample[i, j];
                    }
                    if (sample[i, j] < min)
                    {
                        min = sample[i, j];
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
            if (a2 * r_average > 0)
            {
                x_ucl = x_average + a2 * r_average;
                x_lcl = x_average - a2 * r_average;
            }
            else
            {
                x_ucl = x_average - a2 * r_average;
                x_lcl = x_average + a2 * r_average;
            }
            r_cl = r_average;
            if (d4 * r_average >= d3 * r_average)
            {
                r_ucl = d4 * r_average;
                r_lcl = d3 * r_average;
            }
            else
            {
                r_ucl = d3 * r_average;
                r_lcl = d4 * r_average;
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

        private void ShowDataResult()
        {
            show_xcl.Text = Math.Round(x_cl, 6).ToString();
            show_xucl.Text = Math.Round(x_ucl, 6).ToString();
            show_xlcl.Text = Math.Round(x_lcl, 6).ToString();
            show_rcl.Text = Math.Round(r_cl, 6).ToString();
            show_rucl.Text = Math.Round(r_ucl, 6).ToString();
            show_rlcl.Text = Math.Round(r_lcl, 6).ToString();
        }

        private void ShowChart()
        {
            xchart.Series[0].Points.Clear();
            xchart.Series[1].Points.Clear();
            xchart.Series[2].Points.Clear();
            xchart.Series[3].Points.Clear();
            xchart.Series[4].Points.Clear();
            //xchart.Series[5].Points.Clear();
            //xchart.Series[6].Points.Clear();

            rchart.Series[0].Points.Clear();
            rchart.Series[1].Points.Clear();
            rchart.Series[2].Points.Clear();
            rchart.Series[3].Points.Clear();
            rchart.Series[4].Points.Clear();

            for (int i = 0; i < m; i++)
            {
                xchart.Series[0].Points.Add();
                xchart.Series[1].Points.Add();
                xchart.Series[2].Points.Add();
                xchart.Series[3].Points.Add();
                //xchart.Series[5].Points.Add();

                rchart.Series[0].Points.Add();
                rchart.Series[1].Points.Add();
                rchart.Series[2].Points.Add();
                rchart.Series[3].Points.Add();
                rchart.Series[4].Points.Add();
            }
            for (int i = 0; i < m; i++)
            {
                xchart.Series[0].Points[i].XValue = i + 1;
                xchart.Series[0].Points[i].YValues = new double[1];
                xchart.Series[0].Points[i].YValues[0] = x_cl;

                xchart.Series[1].Points[i].XValue = i + 1;
                xchart.Series[1].Points[i].YValues = new double[1];
                xchart.Series[1].Points[i].YValues[0] = x_ucl;

                xchart.Series[2].Points[i].XValue = i + 1;
                xchart.Series[2].Points[i].YValues = new double[1];
                xchart.Series[2].Points[i].YValues[0] = x_lcl;

                xchart.Series[3].Points[i].XValue = i + 1;
                xchart.Series[3].Points[i].YValues = new double[1];
                xchart.Series[3].Points[i].YValues[0] = x[i];

                //xchart.Series[5].Points[i].XValue = i + 1;
                //xchart.Series[5].Points[i].YValues = new double[1];
                //xchart.Series[5].Points[i].YValues[0] = x[i];

                rchart.Series[0].Points[i].XValue = i + 1;
                rchart.Series[0].Points[i].YValues = new double[1];
                rchart.Series[0].Points[i].YValues[0] = r_cl;

                rchart.Series[1].Points[i].XValue = i + 1;
                rchart.Series[1].Points[i].YValues = new double[1];
                rchart.Series[1].Points[i].YValues[0] = r_ucl;

                rchart.Series[2].Points[i].XValue = i + 1;
                rchart.Series[2].Points[i].YValues = new double[1];
                rchart.Series[2].Points[i].YValues[0] = r_lcl;

                rchart.Series[3].Points[i].XValue = i + 1;
                rchart.Series[3].Points[i].YValues = new double[1];
                rchart.Series[3].Points[i].YValues[0] = r[i];

                rchart.Series[4].Points[i].XValue = i + 1;
                rchart.Series[4].Points[i].YValues = new double[1];
                rchart.Series[4].Points[i].YValues[0] = r[i];
            }

        }


    }
}
