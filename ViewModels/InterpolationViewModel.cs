using ProgramNumericalMet.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.ViewModels
{

    public class InterpolationViewModel : ViewModelBase
    {
        int maxN = 4; //n
        double _a = -0.9;
        double _b = 0.9;
        double step; //шаг на отрезке
        List<double> list_xj; //лист значений xj
        List<List<double>> list_yi;//двухмерный массив значений y и разностей
        List<Interpol> list_itog; //итоговый лист значений


        public int MaxN { get => maxN; set => this.RaiseAndSetIfChanged(ref maxN, value); }
        public double a { get => _a; set => _a = value; }
        public double b { get => _b; set => _b = value; }
        public double Step { get => step; set => step = value; }
        public List<double> List_xj { get => list_xj; set => this.RaiseAndSetIfChanged(ref list_xj, value); }
        public List<List<double>> List_yi { get => list_yi; set => this.RaiseAndSetIfChanged(ref list_yi, value); }
        public List<Interpol> List_itog { get => list_itog; set => this.RaiseAndSetIfChanged(ref list_itog, value); }

        public InterpolationViewModel()
        {

        }
        public void Second_Interpol() //Основной метод рассчета итоговой таблицы
        {
            AddElToList(); 
            Step = (b - a) / MaxN; //Рассчет шага
            List_yi = new List<List<double>>(); //Создание нового двухмерного листа листов
            int count_j = MaxN; //Шаг, чтобы основной не изменялся
            for (int i = 0; i <= MaxN; i++) //Цикл для построения двухмерного массива
            {
                List<double> temp_list_j = new List<double>(); //Создание листа в котором будут элементы
                for (int j = 0; j <= count_j; j++) //Второй цикл
                {
                    double xi = a + j * step; //рассчет х от шага в отрезке
                    if (j == 0) xi = a; //Принудительный х для начала и конца отрезка, чтобы не было случайных хвостов
                    if (j == count_j) xi = b;
                    if (i == 0) //Для первого столба
                    {
                        temp_list_j.Add(ReturnFunc(xi)); //Рассчет y(x)
                    }
                    else //Для отслаьных столбцов
                    {
                        double delta_y = List_yi[i - 1][j + 1] - List_yi[i - 1][j];
                        temp_list_j.Add(delta_y);
                    }
                }
                List_yi.Add(temp_list_j); //Добавление листа в лист
                count_j--; //Уменьшение n, т.к. двухмерный массив в виде треугольника
                if (count_j < 0) break; //Условие для выхода
            }

            List<Interpol> temp_list_itog = new List<Interpol>(); //Создание листа итогов
            for (int i = 0; i < List_xj.Count; i++) //Цикл заполнения листа итогов
            {
                temp_list_itog.Add(new Interpol
                {
                    Id = i,
                    Xj = List_xj[i],
                    F = ItogFuncXj(List_xj[i]),
                    delta = ReturnFunc(List_xj[i]) - ItogFuncXj(List_xj[i])
                });
            }
            List_itog = temp_list_itog;
        }
        public double ItogFuncXj(double x) //Метод рассчета F(xj)
        {
            int count_j = MaxN; //Шаг, чтобы основной не изменялся
            double FuncSum = 0; //Начальная сумма
            for (int i = 0; i <= MaxN; i++) //Цикл рассчета по формуле
            {
                if (i == 0)
                {
                    FuncSum += List_yi[i][count_j];
                }
                else if (i == 1)
                {
                    FuncSum += Q(x) * List_yi[i][count_j];
                }
                else
                {
                    FuncSum += SumFactQ(x, i) * List_yi[i][count_j];
                }
                count_j--;
                if (count_j < 0) break; //Условие для выхода
            }
            return FuncSum;
        }
        public void AddElToList() //Добавление элементов xj в лист
        {
            List_xj = new List<double>(); //Создание нового листа
            double start = 0.70;
            double end = 0.98;
            double step = 0.02; //Начальные значения
            int segmentsCount = (int)Math.Round((end - start) / step) + 1; //Рассчет кол-во элементов в будущем листе
            for (int i = 0; i < segmentsCount; i++) //Цикл заполнения листа
            {
                double xj = start + (i * step);
                List_xj.Add(Math.Round(xj, 2));
            }
        }
        public double Q(double x) //Рассчет q
        {
            return (x - b) / Step;
        }
        public double SumFactQ(double x, int n) //Рассчет значения коэффициента q(q+n-1)/n!
        {
            if (n < 0) return 0; //Проверка n
            double sum = 1;
            double q = Q(x); 
            for (int i = 1; i < n; i++)
            {
                sum *= (q + i);
            }
            double result = (q * sum) / Factorial(n);
            return result;
        }
        public double Factorial(int n) //Рассчет факториала
        {
            if(n<0) return 0;
            double result = 1;
            for(int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        public double ReturnFunc(double x) //Рассчет основной функции
        {
            return Math.Pow(x, 2) / (1 - x);
        }
    }
}
