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
        static double fc = f_p + B_t / 2;
        //==//1//==============//
    }
}
