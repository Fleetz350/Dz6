using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
public delegate double Fun(double x, double a);

namespace Domashka6
{
    class Program
    {
        public static double MyFunc1(double x, double a)
        {
            return a * x * x;
        }
        public static double MyFunc2(double x, double a)
        {
            return a * Math.Sin(x);
        }
        public static void Table(Fun F, double a, double x, double b)
        {
            Console.WriteLine("----- A -------- X -------- Y ----");
            while (x <= b)
            {
                Console.WriteLine("| {0,8:0.000} | {1,8:0.000} | {2,8:0.000} |", a, x, F(x, a));
                x += 1;
            }
            Console.WriteLine("----------------------------------");


        }

        static void Task1()
        {
            Console.Title = "Задание 1";
            // Создаем делегат и передаем ссылку на него в метод Table
            Console.WriteLine("Таблица функции a*x^2:");
            Table(new Fun(MyFunc1), 3.5, -2, 2);
            Console.WriteLine("Таблица функции a*sin(x):");
            Table(new Fun(MyFunc2), 4.5, -2, 2);

            //Console.WriteLine("Еще раз та же таблица, но вызов организован по новому");



        }


        public static double Min(Fun F,double a, double x, double b, double h)
        {
            double y;
            double min = F(x, a);
            while (x <= b)
            {
                y = F(x, a);
                if (y < min) min = y;
                x += h;
            }


            return min;
        }

        public static void SaveFunc(string fileName, Fun F, double a, double xmin, double xmax, double xh)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            double x = xmin;
            
            double count = (xmax - xmin) / xh;
            if (xmax == xmin + Math.Truncate( count) * xh) count++;
            count = Math.Truncate(count);
            bw.Write(count); 
            
            while (x <= xmax)
            {
                bw.Write(F(x, a));
                x += xh;
            }
 
            bw.Close();
            fs.Close();
        }
        public static double[] Load(string fileName, out double min )
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);
            min = double.MaxValue;
            
            double[] d = new double[fs.Length / sizeof(double)-1];

            double len = bw.ReadDouble();//fs.Length / sizeof(double)-1;

            //Console.WriteLine($"Размер массива {len}");
            
            for (int i = 0; i < len; i++)
            {
                // Считываем значение и переходим к следующему
              //  Console.WriteLine($"i = {i}");
                d[i] = bw.ReadDouble();
              //  Console.WriteLine($"d[{i}] = {d[i]}");
                if (d[i] < min) min = d[i];
            }
            bw.Close();
            fs.Close();
            return d;
        }

        static void Task2()
        {
            Console.Title = "Задание 2";
                       
            bool res = false;
            int n = 1;
            Fun[] process = {new Fun(MyFunc1), new Fun(MyFunc2) };
            string[] nameFunc = { "y=a*x^2", "y=a*sin(x)" };
            double a = 3.5; // Коэффицент функции
            // Добавить ввод с консоли параметров функций Xmin, Xmax, h
            Console.WriteLine("Введите значения для функции:");
            Console.Write("Минимальное значение x: ") ;
            double Xmin = 0;
            while (!res)
            {
                res = double.TryParse(Console.ReadLine(), out Xmin);
                if (!res)
                Console.WriteLine("Введено некорректное значение. Повторите ввод: ");
            }
            res = false;
            Console.Write("Максимальное значение x: ");
            double Xmax = 0;
            while (!res || Xmax < Xmin)
            {
                res = double.TryParse(Console.ReadLine(), out Xmax);
                if (!res || Xmax < Xmin)
                Console.WriteLine("Введено некорректное значение. Повторите ввод: ");
            }
            res = false;
            Console.Write("Шаг h: ");
            double xh = 0;
            while (!res || xh <= 0)
            {
                res = double.TryParse(Console.ReadLine(), out xh);
                if (!res || xh <= 0)
                Console.WriteLine("Введено некорректное значение. Повторите ввод: ");
            }

            Console.WriteLine("Выберете функцию: 1 - y=a*x^2; 2 - y=a*sin(x); 0 - выход ");
            res = false;
            string filename = AppDomain.CurrentDomain.BaseDirectory + "function.bin";
            double min;
            while (n != 0 || !res)
            {              
                res = int.TryParse(Console.ReadLine(), out n);
                if (n >= 1 && n <= process.Length)
                {   
                     Console.WriteLine($"Минимум функции задание 2А {nameFunc[n-1]}: {Min(process[n - 1], a, Xmin, Xmax, xh)}");
                     Console.WriteLine("------------------------");                                 
                     SaveFunc(filename, process[n - 1], a, Xmin, Xmax, xh);
                     double[] arr = Load(filename, out min);
                     Console.WriteLine($"Минимум функции задание 2Б {nameFunc[n - 1]}: {min}");
                }
            }                    
        }
        static void Main(string[] args)
        {
            for (int i = 1; i <= 2; i++)
            {
                switch (i)
                {
                    case 1:
                        Task1();
                        break;
                         case 2:
                              Task2();
                              break;

                         /* case 3:
                              Task3();
                              break;
                        */
                }
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
