using System;
using System.Windows.Forms;
using org.matheval;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Differentiation;
using System.Net;
using MathNet.Numerics.Differentiation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MathNet.Numerics.Optimization;
using System.Text.RegularExpressions;

namespace NewtonMethod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(textBox7.Text.ToLower());
            expression.Bind("x", X);

            decimal value = expression.Eval<decimal>();
            return (double)value;
        }
        private bool IsLinearFunction(string function)
        {
            string pattern = @"^\s*-?\d*\s*\*\s*x\s*([+-]\s*\d+)?\s*$|^\s*x\s*([+-]\s*\d+)?\s*$";
            return Regex.IsMatch(function, pattern);
        }
        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userInput = textBox7.Text.ToLower();
            if (IsLinearFunction(userInput))
            {
                MessageBox.Show("Функция является линейной. Метод Ньютона неприменим.");
            }
            else
            {
                try
                {
                    double a, b, Xi;
                    if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                    {
                        throw new ArgumentException("Некорректные значения входных данных");
                    }
                    if (a >= b)
                    {
                        throw new ArgumentException("a должен быть меньше b");
                    }
                    this.chart1.Series[0].Points.Clear();
                    double x = a;
                    double y;
                    while (x <= b)
                    {
                        y = F(x);
                        this.chart1.Series[0].Points.AddXY(x, y);
                        x += 0.1;
                    }
                    Xi = (int)-Math.Log10(Xi);
                    double Root;
                    if (F(a) * F(b) <= 0)
                    {
                        Root = (a + b) / 2;

                        while (Math.Abs(b - a) > Math.Pow(10, -Xi))
                        {
                            double y1 = F(a), y2 = F(b), y3 = F(Root);
                            if (y1 * y3 < 0)
                            {
                                b = Root;
                            }
                            else if (y2 * y3 < 0)
                            {
                                a = Root;
                            }
                            else
                            {
                                break;
                            }
                            Root = (a + b) / 2;

                        }
                        textBox4.Text = $"{Root:F4}";
                    }
                    else
                    {
                        throw new ArgumentException("Нет корней на этом интервале или их больше одного");
                    }
                    if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                    {
                        throw new ArgumentException("Некорректные значения входных данных");
                    }
                    double max, min;
                    double delta = Xi / 10;
                    //минимум
                    while (b - a >= Xi)
                    {
                        double middle = (a + b) / 2;
                        double lambda = middle - delta, mu = middle + delta;
                        if (F(lambda) < F(mu))
                            b = mu;
                        else
                            a = lambda;
                    }
                    min = (a + b) / 2;

                    //точка максимума
                    if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                    {
                        throw new ArgumentException("Некорректные значения входных данных");
                    }
                    while (b - a >= Xi)
                    {
                        double middle = (a + b) / 2;
                        double lambda = middle - delta, mu = middle + delta;
                        if (F(lambda) > F(mu))
                            b = mu;
                        else
                            a = lambda;
                    }
                    max = (a + b) / 2;

                    textBox5.Text = $"{min:F4}";
                    textBox6.Text = $"{max:F4}";
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
        public void GoldenSectionSearchMin(Func<double, double> f, double StartPoint, double EndPoint, double epsilon)
        {
            double x1, x2, k1, k2, F1, F2, Result;
            int count = 0;
            k2 = (Math.Sqrt(5) - 1) / 2;
            k1 = 1 - k2;
            x1 = StartPoint + k1 * (EndPoint - StartPoint);
            x2 = StartPoint + k2 * (EndPoint - StartPoint);
            try
            {
                F1 = F(x1);
                F2 = F(x2);
                while (true)
                {
                    ++count;
                    if ((EndPoint - StartPoint) < epsilon)
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        textBox5.Text = $"{Result:F4}";
                        break;
                    }
                    else
                    {
                        if (F1 < F2)
                        {
                            EndPoint = x2;
                            x2 = x1;
                            F2 = F1;
                            x1 = StartPoint + k1 * (EndPoint - StartPoint);
                            F1 = F(x1);
                        }
                        else
                        {
                            StartPoint = x1;
                            x1 = x2;
                            F2 = F1;
                            x2 = StartPoint + k2 * (EndPoint - StartPoint);
                            F2 = F(x2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void GoldenSectionSearchMax(Func<double, double> f, double StartPoint, double EndPoint, double epsilon)
        {
            double x1, x2, k1, k2, F1, F2, Result;
            int count = 0;
            k2 = (Math.Sqrt(5) - 1) / 2;
            k1 = 1 - k2;
            x1 = StartPoint + k1 * (EndPoint - StartPoint);
            x2 = StartPoint + k2 * (EndPoint - StartPoint);
            try
            {
                F1 = F(x1);
                F2 = F(x2);
                while (true)
                {
                    ++count;
                    if ((EndPoint - StartPoint) < epsilon)
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        textBox6.Text = $"{Result:F4}";
                        break;
                    }
                    else
                    {
                        if (F1 > F2)
                        {
                            EndPoint = x2;
                            x2 = x1;
                            F2 = F1;
                            x1 = StartPoint + k1 * (EndPoint - StartPoint);
                            F1 = F(x1);
                        }
                        else
                        {
                            StartPoint = x1;
                            x1 = x2;
                            F2 = F1;
                            x2 = StartPoint + k2 * (EndPoint - StartPoint);
                            F2 = F(x2);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public double NewtonMethod(double a, double b, double accuracy)
        {

            double x0 = (a + b) / 2;

            double x1 = x0;
            do
            {
                x0 = x1;
                x1 = x0 - (F(x0) / Differentiate.FirstDerivative(F, x0));
            } while (Math.Abs(F(x0) / Differentiate.FirstDerivative(F, x0)) >= accuracy);

            return x1;

        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }
        /*private void goldenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, Xi;
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                if (a >= b)
                {
                    throw new ArgumentException("Некорректные границы интервала");
                }
                this.chart1.Series[0].Points.Clear();
                double x = a;
                double y;
                while (x <= b)
                {
                    y = F(x);
                    this.chart1.Series[0].Points.AddXY(x, y);
                    x += 0.1;
                }

                GoldenSectionSearchMax(F, a, b, Xi);
                GoldenSectionSearchMin(F, a, b, Xi);
                double root = NewtonMethod(a, b, Xi = (int)-Math.Log10(Xi));
                textBox4.Text = $"{root:F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }*/
    }
}