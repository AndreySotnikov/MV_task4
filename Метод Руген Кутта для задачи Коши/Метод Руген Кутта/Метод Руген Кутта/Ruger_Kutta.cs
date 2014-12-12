﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Rugen_Kutt
{
    public delegate double Func(double x, double y);
    
    public class Ruger_Kutta
    {
        private double A;
        private double B;
        private double C;
        private double Yc;
        private double h_min;
        private double eps_max;
        private Func f;
        private string inFileName;
        private int CountPoint;
        private int CountMinH;
        private int CountBadPoints;
        private int s;

        public static string outFileName = "outFile.txt";

        public Ruger_Kutta(string _inFileName, Func _f)
        {
            inFileName = _inFileName;
            f = _f;
            s = 2;
        }

        private string ReadWord(string str, ref int i)
        { 
            int len = str.Length;
            while ((i < len) && (str[i] == ' '))
                i++;
            if (i == len)
                return "";
            int start = i;
            while ((i < len) && (str[i] != ' '))
                i++;
            return str.Substring(start, i - start);
        }

        public bool LoadFomFile()
        {
                StreamReader TxtIn = new StreamReader(new BufferedStream(new FileStream(inFileName, FileMode.Open, FileAccess.Read)));
                string str = TxtIn.ReadLine();
                str.Trim();
                int index = 0;
                if (!(Double.TryParse(ReadWord(str, ref index), out A) &&
                       Double.TryParse(ReadWord(str, ref index), out B) &&
                       Double.TryParse(ReadWord(str, ref index), out C) &&
                       Double.TryParse(ReadWord(str, ref index), out Yc)))
                    return false;
                if (!((A < B) && ((C == B) || (C == A))))
                    return false;
                str = TxtIn.ReadLine();
                index = 0;
                if (!(Double.TryParse(ReadWord(str, ref index), out h_min) &&
                      Double.TryParse(ReadWord(str, ref index), out eps_max)))
                    return false;
                if ((h_min < 0) || (eps_max < 0))
                    return false;
                TxtIn.Close();


            return true;
        }

        private double CalculationError(double CurY, double x, double y, double h)
        {
            double MoreCool = Fouth(x, y, h); 
            return Math.Abs(MoreCool - CurY) / (Math.Pow(2, s) - 1);
        }

        private double CalcStep(double CurY, double y, double x, double h, ref bool IsBadTry, out double eps)
        {
            eps = Math.Abs(CalculationError(CurY, x, y, h));            
            if (!IsBadTry && (eps < eps_max / Math.Pow(2, s)))
                return h * 2;
            IsBadTry = false;
            while ((Math.Abs(h) > h_min) && (eps > eps_max))
            {
                IsBadTry = true;
                h /= 2;
                eps = Math.Abs(CalculationError(CurY, x, y, h));
            }
            if (Math.Abs(h) <= h_min)
            {
                h =Math.Sign(h)* h_min;
                CountMinH++;
            }
            if (eps > eps_max)
                CountBadPoints++;
            return h;
        }

        private double Second(double x, double y, double h)
        {
            double K1 = h * f(x, y);
            double K2 = h * f(x + h / 2, y + K1 / 2);
            return y + K2; 
        }

        private double Fouth(double x, double y, double h)
        {
            double K1 = h * f(x, y);
            double K2 = h * f(x + h / 4, y + K1 / 4);//double K2 = h * f(x + h / 2, y + K1 / 2);
            double K3 = h * f(x + h / 2, y + K2 / 2);
            double K4 = h * f(x + h, y + K1 - 2 * K2 + K3);//double K4 = h * f(x + h, y + K3);
            //return y + (K1 + 2 * K2 + 2 * K3 + K4) / 6;
            return y + (K1 + 4 * K3 + K4) / 6;
        }

        public double CalcValue(double h, double x)
        {
            if (h > 0)
            {
                return B - (x + h);
            }
            else return (x + h) - B;
        }

        public double CalcVal(double x, double h)
        {
            if (h > 0)
                return B - x;
            else return x - B;
        }

        public void Task()
        {
            if (C == B)
            {
                double tmp = A;
                A = B;
                B = tmp;
            }
            double h = (B - A) / 10;
            double x = A;
            double y = Yc;
            double eps = 0;
            bool Bad = false;
            CountPoint = 0;
            CountMinH = 0;
            CountBadPoints = 0;
            double CurY = Yc;
            double PredH = h;
            StreamWriter TxtOut = new StreamWriter(new BufferedStream(new FileStream(outFileName, FileMode.Create, FileAccess.Write)));
            for (; CalcValue(h,x) > h_min; CountPoint++)
            {
                CurY = Second(x, y, h);
                h = CalcStep(CurY, y, x, PredH, ref Bad, out eps);
                y = CurY;
                x += PredH;
                TxtOut.WriteLine("x = " + x.ToString() + " y = " + y.ToString() + " eps = " + eps.ToString("E2"));
                PredH = h;
            }
            if (CalcVal(x, h) >= 2 * h_min)
            {
                CountPoint += 2;
                CountMinH++;
                h = B - x - Math.Sign(h) * h_min;
                CurY = Second(x, y, h);
                x += h;
                TxtOut.WriteLine("x = " + x.ToString() + " y = " + y.ToString() + " eps = " + CalculationError(CurY, x, y, h).ToString("E2"));
                y = CurY;
                h = Math.Sign(h) * h_min; 
                CurY = Second(x, y, h);
                x += h;
                TxtOut.WriteLine("x = " + B.ToString() + " y = " + y.ToString() + " eps = " + CalculationError(CurY, x, y, h).ToString("E2"));
                y = CurY;
            }
            else
            {
                if (CalcVal(x,h) <= 1.5 * h_min)
                {
                    CountPoint++;
                    h = B - x;
                    CurY = Second(x, y, h);
                    TxtOut.WriteLine("x = " + B.ToString() + " y = " + y.ToString() + " eps = " + CalculationError(CurY, x, y, h).ToString("E2"));
                    y = CurY;
                }
                else
                {
                    CountPoint += 2;
                    CountMinH += 2;
                    h = (B - x) / 2;
                    CurY = Second(x, y, h);
                    TxtOut.WriteLine("x = " + x.ToString() + " y = " + y.ToString() + " eps = " + CalculationError(CurY, x, y, h).ToString("E2"));
                    x += h;
                    y = CurY;
                    CurY = Second(x, y, h);
                    TxtOut.WriteLine("x = " + B.ToString() + " y = " + y.ToString() + " eps = " + CalculationError(CurY, x, y, h).ToString("E2"));
                    y = CurY;
                }
            }
            TxtOut.WriteLine("Количество точек интегрирования " + CountPoint.ToString());
            TxtOut.WriteLine("Число точек, в которых не достигается точность " + CountBadPoints.ToString());
            TxtOut.WriteLine("Количество точек с минимальным шагом " + CountMinH.ToString());
            TxtOut.Close();
        }
    }
}
