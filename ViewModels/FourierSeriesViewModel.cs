using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using ProgramNumericalMet.Models;
using ProgramNumericalMet.Views;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Aggregation;

namespace ProgramNumericalMet.ViewModels
{
    public class FourierSeriesViewModel : ViewModelBase
    {
        int _a = 0;
        double _d = 1.5;
        int maxN; //Точность
        int m; //Шаг
        List<Fourier> tableSum; //Итоговый лист значений
        private ObservableCollection<ObservablePoint> staticPoints; //Массив точек для графика

        public int a { get => _a; set => _a = value; }
        public double d { get => _d; set => _d = value; }
        public int MaxN { get => maxN; set => this.RaiseAndSetIfChanged(ref maxN, value); }
        public int M { get => m; set => this.RaiseAndSetIfChanged(ref m, value); }
        public List<Fourier> TableSum { get => tableSum; set => this.RaiseAndSetIfChanged(ref tableSum, value); }

        public Axis[] XAxes { get; set; } //Макс/мин границы по х
        public ISeries[] mySeries; //Для графика
        public ISeries[] MySeries { get => mySeries; set => this.RaiseAndSetIfChanged(ref mySeries, value); }

        public FourierSeriesViewModel()
        {
            MySeries = Array.Empty<ISeries>(); //Создание статичного графика 
            staticPoints = new ObservableCollection<ObservablePoint>();
            XAxes = new Axis[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 1.6,
                }
            };
            for (double x = a; x <= d; x += 0.01) //Заполнение точек х,у
            {
                double y = Math.Tan(x);
                staticPoints.Add(new ObservablePoint(x, y));
            }
            MySeries = new ISeries[] 
            {
                new LineSeries<ObservablePoint>
                {
                    Values = staticPoints,
                    Name = "tg(x)",
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Gray,2)
                }
            };
        }

        public void ButtonAction() //Метода после активации кнопки
        {
            List<Fourier> TempSum = new List<Fourier>(); //Временный лист значений
            double Step = Math.Round(d / M, 6); //Расчет шага
            List<double> list_an = new List<double>(); //Временный лист an чет
            List<double> list_bn = new List<double>(); //Временный лист bn нечет
            for (int i = 0; i < maxN; i++) //Расчет и заполнение an и bn
            {
                double f_0_an = ReturnZnachFuncTgCos(a, i);
                double f_d_an = ReturnZnachFuncTgCos(d, i);
                double internalSum_an = SumFunctionTgCos(i, Step);
                list_an.Add((2.0 / d) * Step * (((f_0_an + f_d_an) / 2.0) + internalSum_an));

                double f_0_bn = ReturnZnachFuncTgSin(a, i);
                double f_d_bn = ReturnZnachFuncTgSin(d, i);
                double internalSum_bn = SumFunctionTgSin(i, Step);
                list_bn.Add((2.0 / d) * Step * (((f_0_bn + f_d_bn) / 2.0) + internalSum_bn));
            }
            int count = 0; //счетчик
            for (double x = a; x <= d; x = Math.Round((x + Step), 6)) //Цикл рассчетов по х
            {
                if (M <= 0 || MaxN <= 0) return; //Проверка на адекватные значени, если обратное, то цикл не будет делать рассчет
                double fourier_even = list_an[0] / 2.0; //Для четной начальное значение
                double fourier_odd = 0; //Для нечетной начальное значение
                for (int i = 0; i < maxN; i++) //рассчет суммы по точности n
                {
                    if(i > 0) fourier_even += list_an[i] * Math.Cos((i * Math.PI * x) / d); //четная, здесь условие i>0, т.к. начальное значение рассчитывается иначе
                    fourier_odd += list_bn[i] * Math.Sin((i * Math.PI * x) / d); //нечетная
                }
                TempSum.Add(new Fourier //Добавление в лист всех значений
                {
                    Id = count + 1,
                    X = x,
                    Y = Math.Round(Math.Tan(x), 6),
                    SumEven = Math.Round(fourier_even, 6), //четная
                    SumOdd = Math.Round(fourier_odd, 6) //Нечетная
                });
                count++; //счетчик
            }
            double[] criticalPoints = { a, d }; //добавление критических точек, которые обязательно должны быть, независимо от шага
            foreach (var point in criticalPoints) //Цикл по крит точкам
            {
                if (TempSum.Find(x => x.X == point) == null) //Если нужной точки нет в листе, выполняются расчеты
                {
                    double x = point;
                    double fourier_even = list_an[0] / 2.0;
                    double fourier_odd = 0;
                    for (int i = 0; i < maxN; i++)
                    {
                        if (i > 0) fourier_even += list_an[i] * Math.Cos((i * Math.PI * x) / d);
                        fourier_odd += list_bn[i] * Math.Sin((i * Math.PI * x) / d);
                    }
                    TempSum.Add(new Fourier
                    {
                        Id = count + 1,
                        X = x,
                        Y = Math.Round(Math.Tan(x), 6),
                        SumEven = Math.Round(fourier_even, 6),
                        SumOdd = Math.Round(fourier_odd, 6)
                    });
                }
            }
            TempSum.Sort((item1, item2) => item1.X.CompareTo(item2.X)); //Соритировка по числам х по возрастанию
            for (int i = 0; i < TempSum.Count; i++) //Восстановление индексов
            {
                TempSum[i].Id = i + 1;
            }
            TableSum = TempSum; 
            var dynamicPoints_an = new ObservableCollection<ObservablePoint>(); //Чет график
            var dynamicPoints_bn = new ObservableCollection<ObservablePoint>(); //Нечет график
            foreach (var item in TableSum) //Цикл по элементам листа
            {
                dynamicPoints_an.Add(new ObservablePoint(item.X, item.SumEven)); //Добавление точек четного графика
                dynamicPoints_bn.Add(new ObservablePoint(item.X, item.SumOdd)); //Добавление точек нечет графика
            }
            MySeries = new ISeries[] //Вывод всех графиков в одном поле
            {
                new LineSeries<ObservablePoint> 
                {
                    Values = staticPoints,
                    Name = "Статическая f(x)",
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Gray, 2)
                },
                new LineSeries<ObservablePoint>
                {
                    Values = dynamicPoints_an,
                    Name = "Четная f(x)",
                    GeometrySize = 0, 
                    Fill = null, 
                    Stroke = new SolidColorPaint(SKColors.Green, 2)
                },
                 new LineSeries<ObservablePoint>
                {
                    Values = dynamicPoints_bn,
                    Name = "Нечетная f(x)",
                    GeometrySize = 0, 
                    Fill = null, 
                    Stroke = new SolidColorPaint(SKColors.Purple, 2)
                }
            };
        }
        public double SumFunctionTgCos(int j, double step) //Метод вычисления суммы значений чет
        {
            double sum = 0;
            for (int i = 1; i <= M - 1; i++)
            {
                double x = step * i;
                sum += ReturnZnachFuncTgCos(x, j);
            }
            return sum;
        }
        public double ReturnZnachFuncTgCos(double x, int i) //Метод вычисления n-ого элемента чет
        {
            return Math.Tan(x) * Math.Cos((i * Math.PI * x) / d);
        }
        public double SumFunctionTgSin(int j, double step) //Метод вычисления суммы значений нечет
        {
            double sum = 0;
            for (int i = 1; i <= M - 1; i++)
            {
                double x = step * i;
                sum += ReturnZnachFuncTgSin(x, j);
            }
            return sum;
        }
        public double ReturnZnachFuncTgSin(double x, int i) //Метод вычисления n-ого элемента нечет
        {
            return Math.Tan(x) * Math.Sin((i * Math.PI * x) / d);
        }
        public void InFunctionSeries() //переключатель для RadioButton
        {
            MainWindowViewModel.Self.FunctionSeriesVM = new FunctionSeriesViewModel();
            MainWindowViewModel.Self.Page = new FunctionSeriesView();

        }
    }
}
