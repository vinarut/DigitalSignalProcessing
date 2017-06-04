using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ЦОС_курсовая
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeLabels();

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            //chart1.ChartAreas[0].AxisX.Maximum = 1;
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart_analog.ChartAreas[0].AxisX.Minimum = 0;

            for (int i = 0; i < F_kvant.Length; i++)
                chart1.Series[0].Points.AddXY(i * T_d, F_kvant[i]);
            for (double i = 0; i <= 1; i += 0.01)
                chart_analog.Series[0].Points.AddXY(i, F(i));
            for (int i = 0; i < F_disk.Length; i++)
                chart3.Series[0].Points.AddXY((double)i / Fs, S_vyh[i]);

            Hw2();
            Hw_triangle();
            Hw_Ham();
            Hw_Black();
            vosst();
        }

        private void InitializeLabels()
        {
            label_Fp.Text = "Fp = " + f_p + " Гц";
            label_Fa.Text = "Fa = " + f_a + " Гц";
            label_Ap.Text = "Ap = " + A_p + " дБ";
            label_Aa.Text = "Aa = " + A_a + " дБ";

            label_f1.Text = "f1 = " + f1 + " Гц";
            label_f2.Text = "f2 = " + f2 + " Гц";
            label_f3.Text = "f3 = " + f3 + " Гц";
            label_f4.Text = "f4 = " + f4 + " Гц";
            label_f5.Text = "f5 = " + f5 + " Гц";

            label_Fs.Text = "Fs = " + Fs + " Гц";
            label_Td.Text = "Td = " + T_d * 1E3 + " мс";
            label_delta_kvant.Text = "Δ = " + (delta_kvant * 1E3).ToString("0.000") + " мВ";
        }

        //=====================//
        // Вариант 1
        //=====================//
        //static int f_p = 5; // Гц
        //static int f_a = 8; // Гц
        //static int A_p = 1;  // дБ
        //static int A_a = 30; // дБ 
        //=====================//
        //static int f1 = 1;  // Гц
        //static int f2 = 5; // Гц
        //static int f3 = 7; // Гц
        //static int f4 = 8; // Гц
        //static int f5 = 30; // Гц
        //=====================//
        // Вариант 5
        //=====================//
        static int f_p = 25; // Гц
        static int f_a = 40; // Гц
        static int A_p = 1;  // дБ
        static int A_a = 30; // дБ
        //=====================//
        static int f1 = 1;  // Гц
        static int f2 = 25; // Гц
        static int f3 = 30; // Гц
        static int f4 = 40; // Гц
        static int f5 = 60; // Гц
        //=====================//
        //=====================//
        // Вариант 13
        //=====================//
        //static int f_p = 8; // Гц
        //static int f_a = 12; // Гц
        //static int A_p = 2;  // дБ
        //static int A_a = 30; // дБ
        //=====================//
        //static int f1 = 1;  // Гц
        //static int f2 = 8; // Гц
        //static int f3 = 10; // Гц
        //static int f4 = 12; // Гц
        //static int f5 = 50; // Гц
        //=====================//
        
        //==//1//===Для ФНЧ====//
        static double delta_1 = (Math.Pow(10, 0.05 * A_p) - 1) / (Math.Pow(10, 0.05 * A_p) + 1);
        static double delta_2 = Math.Pow(10, -0.05 * A_a);
        static double delta = Math.Min(delta_1, delta_2);
        //=====================//
        static double B_t = f_a - f_p;
        static double f_c = f_p + B_t / 2;
        //==//1//==============//

        //==//2//==============//
        static double A = -20 * Math.Log10(delta);
        static double D = find_D(A);
        static double alpha = find_alpha(A);
        static int Fs = 128;
        
        static double find_D(double A)
        {
            if (A <= 21)
                return 0.9222;
            else
                return (A - 7.95) / 14.36;
        }

        static double find_alpha(double A)
        {
            if (A <= 21)
                return 0;
            else if (A <= 50)
                return 0.5842 * Math.Pow(A - 21, 0.4) + 0.07886 * (A - 21);
            else
                return 0.1102 * (A - 8.7);            
        }

        //static double round_M(double M)
        //{
        //    if (M >= Math.Floor(M) + 0.5)
        //        return Math.Floor(M) + 1;
        //    else
        //        return Math.Floor(M);
        //}

        static double M = Math.Round(Fs * D / B_t);
        static double N = M + 1;
        //=====================//

        //==//3//==============//
        static double[] a = coef_Furie((int)M);

        static double[] coef_Furie(int M)
        {
            double[] a = new double[M / 2 + 1];

            if (M % 2 == 0)
            {
                a[0] = 2 * f_c / Fs;
                for (int i = 1; i < a.Length; i++)
                    a[i] = Math.Sin(2 * Math.PI * i * f_c / Fs) / (Math.PI * i);                
            }
            else
            {
                for (int i = 0; i < a.Length; i++)
                    a[i] = Math.Sin(2 * Math.PI * (i - 0.5) * f_c / Fs) / (Math.PI * (i - 0.5));
            }

            return a;
        }
        //=====================//

        //==//4//==============//
        static double[] a_k = a_Kaiser((int)M);
        static double[] a_Kaiser(int M)
        {
            double[] a_k = new double[M / 2 + 1];
            for (int i = 0; i < a_k.Length; i++)
            {
                double beta = alpha * Math.Sqrt(1 - Math.Pow(2 * i / M, 2));
                double w = I0(beta) / I0(alpha);
                a_k[i] = a[i] * w; 
            }
            return a_k;
        }
        static double I0(double x) 
        {   
            double I = 1;
            for (int k = 1; k <= 10; k++)
                I += Math.Pow(Math.Pow(x / 2, k) / factorial(k), 2);
            return I;
        }
        static double factorial(int p)
        {
            for (int i = p - 1; i >= 1; i--)
                p *= i;
            return p;
        }
        //=====================//

        //==//5//==============//
        static double[] h = find_h((int)M, a_k);

        static double[] find_h(int M, double[] a_k)
        {
            h = new double[M];
            for (int i = 0; i <= M / 2 - 1; i++)
                h[i] = a_k[M / 2 - i];

            h[M / 2] = a_k[0];

            for (int i = M / 2 + 1; i < M; i++)
                h[i] = h[M - i];

            return h;
        }
        //=====================//

        //==//6//==============//
        static double[] H_w = Hw((int)M);
        static double[] Hw(int M)
        {
            double[] H = new double[f_p + 1];
            double Re, Im;

            for (int w = 0; w < H.Length; w++)
            {
                Re = 0;
                Im = 0;
                for (int n = 1; n <= M; n++)
                {
                    Re += h[n - 1] * Math.Cos(n * w);
                    Im += h[n - 1] * Math.Sin(n * w);
                    
                }
                H[w] = Math.Sqrt(Re * Re + Im * Im);
            }
            return H;
        }
        void Hw2()
        {
            double[] H = new double[f_p + 1];
            double Re, Im;
            chart_Kaiser.ChartAreas[0].AxisX.Minimum = 0;
            chart_Kaiser.ChartAreas[0].AxisX.Maximum = f_p;

            for (double w = 0; w < f_p; w+=0.1)
            {
                Re = 0;
                Im = 0;
                for (int n = 1; n <= M; n++)
                {
                    Re += h[n - 1] * Math.Cos(n * w);
                    Im += h[n - 1] * Math.Sin(n * w);

                }
                chart_Kaiser.Series[0].Points.AddXY(w, Math.Sqrt(Re * Re + Im * Im));
            }
        }
        //=====================//

        //==//Дискретизация//==//
        static double T = 1;
        static double T_d = 1.0 / Fs;
        static double N_d = 1 + T / T_d;
        static double[] F_disk = diskret((int)N_d);

        static double F(double t)
        {
            //return Math.Cos(t * f1 * 2 * Math.PI);
            return Math.Cos(t * f1 * 2 * Math.PI) + Math.Cos(t * f2 * 2 * Math.PI) + Math.Cos(t * f3 * 2 * Math.PI) + Math.Cos(t * f4 * 2 * Math.PI) + Math.Cos(t * f5 * 2 * Math.PI);
        }

        static double[] diskret(int N)
        {
            double[] array = new double[N - 1];
            //double[] array = new double[129];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = F(i * T_d);
            }

            return array;
        }
        //=====================//

        //==//Квантование//===//
        static double delta_kvant = (Smax() - Smin()) / 63;
        static double[] F_kvant = kvant((int)N_d);
        static double[] kvant(int N)
        {
            double[] array = new double[N - 1];
            //double Skv = 0;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Math.Floor(F_disk[i] / delta_kvant) * delta_kvant;
                //if (F_disk[i] == 0)
                //    array[i] = F_disk[i];
                //if (F_disk[i] > 0)
                //{   
                //    Skv = 0;
                //    do { Skv += delta_kvant; }
                //    while (Math.Abs(Skv - F_disk[i]) > delta_kvant);
                //    array[i] = Skv;
                //}
                //if(F_disk[i] < 0)
                //{
                //    Skv = 0;
                //    do { Skv -= delta_kvant; }
                //    while (Math.Abs(Skv - F_disk[i]) > delta_kvant);
                //    array[i] = Skv;
                //}
            }
            return array;
        }
        static double Smax()
        {
            double max = F_disk[0];
            for (int i = 1; i < F_disk.Length; i++)
                if (F_disk[i] > max)
                    max = F_disk[i];

            return max;
        }
        static double Smin()
        {
            double min = F_disk[0];
            for (int i = 1; i < F_disk.Length; i++)
                if (F_disk[i] < min)
                    min = F_disk[i];

            return min;
        }
        static double[] S_vyh = filtr((int)N_d);
        static double[] filtr(int N)
        {
            double[] array = new double[N - 1];
            double S = 0;
            for (int k = 0; k < array.Length; k++)
            {
                S = 0;
                for(int i = 0; i <= k; i++)
                {
                    if (k - i < h.Length)
                        S += F_kvant[i] * h[k - i];
                }
                array[k] = S;
            }
            return array;
        }
        //=====================//

        //==//Окна//===========//

        //Треугольное окно
        static double[] triangle_win()
        {
            double[] a_t = new double[a.Length];
            for (int i = 0; i < a_t.Length; i++)
                a_t[i] = (1 - i / N) * a[i];

            return a_t;
        }
        void Hw_triangle()
        {
            double[] h = find_h((int)M, triangle_win());
            chart_triangle.ChartAreas[0].AxisX.Minimum = 0;
            chart_triangle.ChartAreas[0].AxisX.Maximum = f_p;
            double Re, Im;

            for (double w = 0; w < f_p; w += 0.1)
            {
                Re = 0;
                Im = 0;
                for (int n = 1; n <= M; n++)
                {
                    Re += h[n - 1] * Math.Cos(n * w);
                    Im += h[n - 1] * Math.Sin(n * w);

                }
                chart_triangle.Series[0].Points.AddXY(w, Math.Sqrt(Re * Re + Im * Im));
            }
        }
        //Окно Хэмминга
        static double[] Hamm_win()
        {
            double[] a_h = new double[a.Length];
            double alpha = 0.54;
            for (int i = 0; i < a_h.Length; i++)
                a_h[i] = (alpha - (1 - alpha) * Math.Cos(Math.PI * 2 * i / N)) * a[i];

            return a_h;
        }
        void Hw_Ham()
        {
            double[] h = find_h((int)M, Hamm_win());
            chart_Ham.ChartAreas[0].AxisX.Minimum = 0;
            chart_Ham.ChartAreas[0].AxisX.Maximum = f_p;
            double Re, Im;

            for (double w = 0; w < f_p; w += 0.1)
            {
                Re = 0;
                Im = 0;
                for (int n = 1; n <= M; n++)
                {
                    Re += h[n - 1] * Math.Cos(n * w);
                    Im += h[n - 1] * Math.Sin(n * w);

                }
                chart_Ham.Series[0].Points.AddXY(w, Math.Sqrt(Re * Re + Im * Im));
            }
        }
        //Окно Блэкмана
        static double[] Black_win()
        {
            double[] a_b = new double[a.Length];
            for (int i = 0; i < a_b.Length; i++)
                a_b[i] = (0.42 - 0.5 * Math.Cos(Math.PI * 2 * i / N) + 0.08 * Math.Cos(Math.PI * 4 * i / N)) * a[i];

            return a_b;
        }
        void Hw_Black()
        {
            double[] h = find_h((int)M, Black_win());
            chart_Black.ChartAreas[0].AxisX.Minimum = 0;
            chart_Black.ChartAreas[0].AxisX.Maximum = f_p;
            double Re, Im;

            for (double w = 0; w < f_p; w += 0.1)
            {
                Re = 0;
                Im = 0;
                for (int n = 1; n <= M; n++)
                {
                    Re += h[n - 1] * Math.Cos(n * w);
                    Im += h[n - 1] * Math.Sin(n * w);

                }
                chart_Black.Series[0].Points.AddXY(w, Math.Sqrt(Re * Re + Im * Im));
            }
        }
        //=====================//

        //==//Восстановление//=//
        void vosst()
        {
            double[] C = new double[(int)((N_d - 1) / 2)];
            double[] fi = new double[C.Length];

            double N = N_d - 1, Re, Im;

            for (int i = 0; i < S_vyh.Length; i++)
            {
                C[0] += S_vyh[i];
                C[0] *= (1 / N);
            }

            for (int i = 1; i < C.Length; i++)
            {
                Re = 0;
                Im = 0;
                for (int j = 0; j < S_vyh.Length; j++)
                {
                    Re += S_vyh[j] * Math.Cos(2 * Math.PI * i * j / N);
                    Im += S_vyh[j] * Math.Sin(2 * Math.PI * i * j / N);
                }
                Re *= (1 / N);
                Im *= (1 / N);
                C[i] = Math.Sqrt(Re * Re + Im * Im);
                fi[i] = Math.Atan(Im / Re);
            }

            chart_vosst.ChartAreas[0].AxisX.Minimum = 0;
            double t = 1.0 / S_vyh.Length, S;
            for (int i = 0; i < S_vyh.Length; i++)
            {
                S = 0;
                for (int j = 1; j < C.Length - 1; j++)
                    S += 2 * C[j] * Math.Cos(2 * j * Math.PI * i * t + fi[j]);
                S += C[0];
                S += C[C.Length - 1] * Math.Cos((C.Length - 1) * Math.PI * i * t + fi[fi.Length - 1]);
                chart_vosst.Series[0].Points.AddXY(i * t, S);
            }

        }
    }
}
