using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NewtonMethod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, exp;

                if (!double.TryParse(textBox1.Text, out a))
                {
                    MessageBox.Show("В а должно быть только натуральное число.");

                    return;
                }

                if (!double.TryParse(textBox2.Text, out b))
                {
                    MessageBox.Show("В b должно быть только натуральное число.");

                    return;
                }

                if (a > b)
                {
                    MessageBox.Show("a не может быть больше b");

                    return;
                }

                if (!double.TryParse(textBox3.Text, out exp) || !Regex.IsMatch(textBox3.Text, @"(1|10+)|(0,(1|0+1))")
                    || textBox3.Text[0] == '-')
                {
                    MessageBox.Show("неправильный формат. пример: 0,1");

                    return;
                }
                Func<double, double> function = x => (27 - 18 * x + 2 * Math.Pow(x, 2)) * Math.Exp(-(x / 3));
                Func<double, double> derivative = x => (4*x-18)*Math.Exp(-(x/3)) - ((2 * Math.Pow(x, 2) - 18 * x + 27) * Math.Exp(-(x / 3)))/3;

                double x0 = (a + b) / 2;
                double x1 = x0;
                

                if (function(a) * function(b) <= 0)
                {
                    do
                    {
                        x0 = x1;
                        x1 = x0 - (function(x0) / derivative(x0));
                    } while (Math.Abs(function(x0) / derivative(x0)) >= exp);
                    textBox4.Text = $"{x1}:F4";//x1.ToString();
                    chart1.Series.Clear();
                    Series series = new Series("Function");
                    series.ChartType = SeriesChartType.Line;

                    for (double x = a; x <= b; x += 0.1)
                    {
                        double y = function(x);
                        series.Points.AddXY(x, y);
                    }

                    chart1.Series.Add(series);
                } else
                {
                    MessageBox.Show("Отсутствуют корни на данном отрезке");
                }
                               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            chart1.Series[0].Points.Clear();
            //textBox1.Text = string.Empty;
        }
    }
}