using ProgramNumericalMet.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.ViewModels
{
    public class ApproximationViewModel : ViewModelBase
    {
        int maxN = 4; //n
        int maxM = 14; //m
        int _c = 3;
        double _a = -4;
        double _b = 4;
        double step; //шаг на отрезке
        double s;
        List<double> list_func;
        List<double> array_B;
        List<double> array_C;
        List<double> array_X;
        List<List<double>> matrix_A;
        List<Approx> approxes;

        private Random rnd = new Random();

        public int MaxN { get => maxN; set => this.RaiseAndSetIfChanged(ref maxN, value); }
        public int MaxM { get => maxM; set => this.RaiseAndSetIfChanged(ref maxM, value); }
        public int c { get => _c; set => this.RaiseAndSetIfChanged(ref _c, value); }
        public double a { get => _a; set => _a = value; }
        public double b { get => _b; set => _b = value; }
        public double Step { get => step; set => step = value; }
        public double S { get => s; set => this.RaiseAndSetIfChanged(ref s, value); }
        public List<Approx> Approxes { get => approxes; set => this.RaiseAndSetIfChanged(ref approxes, value); }
        public List<double> List_func { get => list_func; set => this.RaiseAndSetIfChanged(ref list_func, value); }
        public List<double> Array_B { get => array_B; set => this.RaiseAndSetIfChanged(ref array_B, value); }
        public List<double> Array_C { get => array_C; set => this.RaiseAndSetIfChanged(ref array_C, value); }
        public List<double> Array_X { get => array_X; set => this.RaiseAndSetIfChanged(ref array_X, value); }
        public List<List<double>> Matrix_A { get => matrix_A; set => this.RaiseAndSetIfChanged(ref matrix_A, value); }



        public ApproximationViewModel()
        {

        }
        public void ButtonAction()
        {
            if (MaxN <= 0 || MaxM <= 0 || c <= 0) return;
            Step = (b - a) / MaxM;
            Array_X = new List<double>();
            for (int i = 0; i <= MaxM; i++)
            {
                Array_X.Add(a + i * Step);
            }
            AddZnachFunc();
            SolveMatrix();

            for (int j = 0; j <= MaxN; j++) //Цикл составления матрицы для вычисления С
            {
                double max = -99999;
                int max_i = 0, max_j = 0; //Переменные которые хранят в себе max значение и его индексы
                for (int i = j; i <= MaxN; i++) //Цикл для нахождения макс элемента в столбце
                {
                    if (Math.Abs(Matrix_A[i][j]) > max)
                    {
                        max = Math.Abs(Matrix_A[i][j]);
                        max_i = i;
                        max_j = j;
                    }
                }
                List<double> list; //Меняем местами строку с максимальным элементом с j-строкой
                list = Matrix_A[j];
                Matrix_A[j] = Matrix_A[max_i];
                Matrix_A[max_i] = list;

                double cup = Array_B[j]; //Меняем значение B по соответсвтвию
                Array_B[j] = Array_B[max_i];
                Array_B[max_i] = cup;

                for (int k = j; k <= MaxN; k++) //Цикл для зануления не нужных элементов
                {
                    if (k == j) continue;
                    double koef = Matrix_A[k][j] / Matrix_A[j][j]; //Рассчитываем каждый раз коэфициент
                    for (int c = j; c <= MaxN; c++) //Цикл для вычиления новых значений (вычитанние из строки j-строку)
                    {
                        Matrix_A[k][c] = Matrix_A[k][c] - koef * Matrix_A[j][c]; //НУжные элементы зануляются, нужные остаются образуя треугольник
                    }
                    Array_B[k] = Array_B[k] - koef * Array_B[j]; //Меняем значение B соотвественно
                }
            }
            Array_C = new List<double>();
            int count = MaxN;
            for (int i = MaxN; i >= 0; i--)
            {
                double sum = Array_B[i];
                for (int j = count; j <= MaxN; j++)
                {
                    if (i == MaxN && j == MaxN) break;
                    else
                    {
                        sum -= Matrix_A[i][j] * Array_C[MaxN - j];
                    }
                }
                Array_C.Add(sum / Matrix_A[i][i]);
                if (i <= MaxN - 1) count--;
            }
            Array_C.Reverse();

            List<double> list_Y = new List<double>();
            double sumS = 0;
            for (int i = 0; i <= MaxM; i++)
            {
                list_Y.Add(ReturnY(Array_X[i]));
                sumS += Math.Pow(list_Y[i] - List_func[i], 2);
            }
            S = sumS;
            this.RaisePropertyChanged(nameof(Matrix_A));
        }

        public double ReturnY(double x)
        {
            double sum = 0;
            for (int i = 0; i <= MaxN; i++)
            {
                sum += Array_C[i] * Math.Pow(x, i);
            }
            return sum;
        }
        public void SolveMatrix()
        {
            List<double> list_D = new List<double>();
            Matrix_A = new List<List<double>>();

            for (int k = 0; k <= 2 * MaxN; k++)
            {
                double temp_sum = 0;
                for (int i = 0; i <= MaxM; i++)
                {
                    if (k == 0) temp_sum += 1.0;
                    else temp_sum += Math.Pow(Array_X[i], k);
                }
                list_D.Add(temp_sum);
            }
            for (int i = 0; i <= MaxN; i++)
            {
                List<double> temp_list = new List<double>();
                for (int j = 0; j <= MaxN; j++)
                {
                    temp_list.Add(list_D[i + j]);
                }
                Matrix_A.Add(temp_list);
            }
            Array_B = new List<double>();
            for (int k = 0; k <= MaxN; k++)
            {
                double temp_sum = 0;
                for (int i = 0; i <= MaxM; i++)
                {
                    if (k == 0) temp_sum += list_func[i] * 1.0;
                    else temp_sum += list_func[i] * Math.Pow(Array_X[i], k);
                }
                Array_B.Add(temp_sum);
            }
        }
        public void AddZnachFunc()
        {
            List_func = new List<double>();
            for (int i = 0; i <= MaxM; i++)
            {
                double z = RandNumberZ();
                double k = 0;
                if (z < 0.5) k = -1;
                else k = 1;
                double f = ReturnFunc(Array_X[i]) * (1 + (k * z) / c);
                List_func.Add(f);
            }
        }
        public double ReturnFunc(double x)
        {
            return Math.Abs(Math.Abs(x) - 5);
        }
        public double RandNumberZ()
        {
            return rnd.Next(2) == 0 ? 0.0 : 1.0;
        }

    }
}
