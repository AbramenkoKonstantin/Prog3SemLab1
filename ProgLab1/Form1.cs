using System;
using MathNet.Symbolics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ProgLab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                double.TryParse(textBoxA.Text, out double aBord);
                double.TryParse(textBoxB.Text, out double bBord);
                double.TryParse(textBoxE.Text, out double esp);
                Expression func = Infix.ParseOrThrow(textBoxF.Text);
                if (esp < 0)
                {
                    throw new Exception();
                }

                double espValue = esp;
                int counter = 0;
                while (espValue < 1)
                {
                    espValue *= 10;
                    counter += 1;
                }

                LineDraw(aBord, bBord, esp, func);
                double minPoint = 0;
                while (bBord - aBord >= esp)
                {
                    double mid = (aBord + bBord) / 2;
                    double left = mid - esp;
                    double right = mid + esp;
                    if (FuncValue(left, func) < FuncValue(right, func))
                    {
                        bBord = mid;
                    }
                    else
                    {
                        aBord = mid;
                    }
                    minPoint = mid;
                }
                textBoxAnswer.Text = "(" + Math.Round(minPoint, counter).ToString() + ":" + Math.Round(FuncValue(minPoint, func), counter).ToString() + ")";
            }
            catch
            {
                if (textBoxA.Text == "" || textBoxB.Text == "" || textBoxE.Text == "" || textBoxF.Text == "")
                {
                    MessageBox.Show("Пустые поля недопустимы");
                }
                else if (double.Parse(textBoxE.Text) < 0)
                {
                    MessageBox.Show("Точность не может быть меньше 0");
                }
                else if (double.Parse(textBoxA.Text) >= double.Parse(textBoxB.Text))
                {
                    MessageBox.Show("Параметр a должен быть меньше параметра b");
                }
                else
                {
                    MessageBox.Show("Некорректно задана вычисляемая функция");
                }
            }
        }
        private void LineDraw(double aBord, double bBord, double esp, Expression func)
        {
            double espValue = esp;
            int counter = 0;
            while (espValue < 1)
            {
                espValue *= 10;
                counter += 1;
            }

            if ((bBord - aBord) / 50000 > esp)
            {
                esp = (bBord - aBord) / 50000;
            }
            
            funcChart.Series[0].Points.Clear();

            while (aBord < bBord)
            {
                double funcValue = Math.Round(FuncValue(aBord, func), counter);
                funcChart.Series[0].Points.AddXY(aBord, funcValue);
                aBord += esp;
            }
        }
        private double FuncValue(double point, Expression func)
        {
            Dictionary<string, FloatingPoint> x = new Dictionary<string, FloatingPoint>()
            {
                { "x", point }
            };
            return Evaluate.Evaluate(x, func).RealValue;
        }

        private void Params_KeyPress(object sender, KeyPressEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (char.IsDigit(e.KeyChar) || (e.KeyChar == ',' && textBox.Text.Contains(",") == false) || (e.KeyChar == '-' && textBox.Text == "") || (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
