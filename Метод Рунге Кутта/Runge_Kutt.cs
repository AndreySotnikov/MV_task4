using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace Runge_Kutt
{
    public delegate double Func(double x, double y);
    
    public class Runge_Kutt
    {
        private double A;
        private double B;
        private double C;
        private double Yc;
        private double h_min;
        private double eps_max;
        private Func f;
        private string inFileName;
        private int CountPoint = 0;
        private int CountMinH = 0;
        private int CountBadPoints = 0;
        private int s;
        DataGridView dgv;
        Label l1,l2,l3;

        public static string outFileName = "outFile.txt";

        public Runge_Kutt(string _inFileName, Func _f, DataGridView _dgv, Label _l1, Label _l2, Label _l3)
        {
            inFileName = _inFileName;
            f = _f;
            s = 2;
            dgv = _dgv;
            l1 = _l1;
            l2 = _l2;
            l3 = _l3;
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

        private double CalculationError(double x, double y, double h)
        {
            double good = Fouth(x, y, h);
            double bad = Second(x, y, h);
            return Math.Abs(good - bad); // (Math.Pow(2, s) - 1);
        }

        private double CalcStep(double CurY, double y, double x, double h, ref bool IsBadTry, out double eps)
        {
            eps = CalculationError(x, y, h);
            if (!IsBadTry && (eps < eps_max / Math.Pow(2, s)))
                return h * 2;
            IsBadTry = false;
            while ((Math.Abs(h) > h_min) && eps > eps_max)
            {
                IsBadTry = true;
                h /= 2;
                eps = CalculationError(x, y, h);
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
            dgv.RowCount++;
            dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
            dgv.Rows[dgv.RowCount - 1].Cells[1].Value = x;
            dgv.Rows[dgv.RowCount - 1].Cells[2].Value = y;
            dgv.Rows[dgv.RowCount - 1].Cells[3].Value = (0).ToString("E2");
            h = CalcStep(CurY, y, x, PredH, ref Bad, out eps);
            CurY = Second(x, y, h);
            eps = CalculationError(x, y, h);
           /* dgv.RowCount++;
            dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
            dgv.Rows[dgv.RowCount - 1].Cells[1].Value = x;
            dgv.Rows[dgv.RowCount - 1].Cells[2].Value = CurY;
            dgv.Rows[dgv.RowCount - 1].Cells[3].Value = eps.ToString("E2");*/
            PredH = h;
            for (; CalcValue(h,x) > h_min; CountPoint++)
            {
                eps = 0;
                CurY = Second(x, y, h);
                double tmpeps = CalculationError(x, y, h);
                h = CalcStep(CurY, y, x, PredH, ref Bad, out eps);
                y = CurY;
                x += PredH;
                dgv.RowCount++;
                dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
                dgv.Rows[dgv.RowCount - 1].Cells[1].Value = x;
                dgv.Rows[dgv.RowCount - 1].Cells[2].Value = y;
                dgv.Rows[dgv.RowCount - 1].Cells[3].Value = tmpeps.ToString("E2");
                TxtOut.WriteLine("x = " + x.ToString() + "\ty = " + y.ToString() + "\teps = " + tmpeps.ToString("E2"));
                PredH = h;
            }
            if (CalcVal(x, h) >= 2 * h_min)
            {
                CountPoint += 2;
                CountMinH++;
                h = B - x - Math.Sign(h) * h_min;
                CurY = Second(x, y, h);
                x += h;
                ////////////////
                eps = CalculationError(x, y, h);
                dgv.RowCount++;
                dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
                dgv.Rows[dgv.RowCount - 1].Cells[1].Value = x;
                dgv.Rows[dgv.RowCount - 1].Cells[2].Value = CurY;
                dgv.Rows[dgv.RowCount - 1].Cells[3].Value = eps.ToString("E2");
                ////////////////
                TxtOut.WriteLine("x = " + x.ToString() + "\ty = " + CurY.ToString() + "\teps = " + eps.ToString("E2"));
                y = CurY;
                h = Math.Sign(h) * h_min; 
                CurY = Second(x, y, h);
                x += h;
                eps = CalculationError(x, y, h);
                ////////////////
                dgv.RowCount++;
                dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
                dgv.Rows[dgv.RowCount - 1].Cells[1].Value = B;
                dgv.Rows[dgv.RowCount - 1].Cells[2].Value = CurY;
                dgv.Rows[dgv.RowCount - 1].Cells[3].Value = eps.ToString("E2");
                ////////////////
                TxtOut.WriteLine("x = " + B.ToString() + "\ty = " + CurY.ToString() + "\teps = " + eps.ToString("E2"));
                y = CurY;
            }
            else
            {
                if (CalcVal(x,h) <= 1.5 * h_min)
                {
                    CountPoint++;
                    h = B - x;
                    CurY = Second(x, y, h);
                    eps = CalculationError(x, y, h);
                    ////////////////
                    dgv.RowCount++;
                    dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = B;
                    dgv.Rows[dgv.RowCount - 1].Cells[2].Value = CurY;
                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = eps.ToString("E2");
                    ////////////////
                    TxtOut.WriteLine("x = " + B.ToString() + "\ty = " + CurY.ToString() + "\teps = " + eps.ToString("E2"));
                    y = CurY;
                }
                else
                {
                    CountPoint += 2;
                    CountMinH += 2;
                    h = (B - x) / 2;
                    CurY = Second(x, y, h);
                    eps = CalculationError(x, y, h);
                    ////////////////
                    dgv.RowCount++;
                    dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = x;
                    dgv.Rows[dgv.RowCount - 1].Cells[2].Value = CurY;
                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = eps.ToString("E2");
                    ////////////////
                    TxtOut.WriteLine("x = " + x.ToString() + "\ty = " + CurY.ToString() + "\teps = " + eps.ToString("E2"));
                    x += h;
                    y = CurY;
                    CurY = Second(x, y, h);
                    eps = CalculationError(x, y, h);
                    ////////////////
                    dgv.RowCount++;
                    dgv.Rows[dgv.RowCount - 1].Cells[0].Value = dgv.RowCount;
                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = B;
                    dgv.Rows[dgv.RowCount - 1].Cells[2].Value = CurY;
                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = eps.ToString("E2");
                    ////////////////
                    TxtOut.WriteLine("x = " + B.ToString() + "\ty = " + CurY.ToString() + "\teps = " + eps.ToString("E2"));
                    y = CurY;
                }
            }
            l1.Text = CountPoint.ToString();
            l2.Text = CountBadPoints.ToString();
            l3.Text = CountMinH.ToString();
            TxtOut.WriteLine("Количество точек интегрирования " + CountPoint.ToString());
            TxtOut.WriteLine("Число точек, в которых не достигается точность " + CountBadPoints.ToString());
            TxtOut.WriteLine("Количество точек с минимальным шагом " + CountMinH.ToString());
            TxtOut.Close();
        }
    }
}
