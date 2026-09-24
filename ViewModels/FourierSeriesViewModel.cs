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
        int maxN;
        int m; //Шаг
        List<Fourier> tableSum;
        private ObservableCollection<ObservablePoint> staticPoints;

        public int a { get => _a; set => _a = value; }
        public double d { get => _d; set => _d = value; }
        public int MaxN { get => maxN; set => this.RaiseAndSetIfChanged(ref maxN, value); }
        public int M { get => m; set => this.RaiseAndSetIfChanged(ref m, value); }
        public List<Fourier> TableSum { get => tableSum; set => this.RaiseAndSetIfChanged(ref tableSum, value); }

        public Axis[] XAxes { get; set; }
        public ISeries[] mySeries;
        public ISeries[] MySeries { get => mySeries; set => this.RaiseAndSetIfChanged(ref mySeries, value); }

        public FourierSeriesViewModel()
        {
            MySeries = Array.Empty<ISeries>();
            staticPoints = new ObservableCollection<ObservablePoint>();
            XAxes = new Axis[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 1.6,
                }
            };
            for (double x = a; x <= d; x += 0.01)
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

        public void ButtonAction()
        {
            List<Fourier> TempSum = new List<Fourier>();
            double Step = Math.Round(d / M, 6);
            List<double> list_an = new List<double>();
            List<double> list_bn = new List<double>();
            for (int i = 0; i < maxN; i++)
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
            int count = 0;
            for (double x = a; x <= d; x = Math.Round((x + Step), 6))
            {
                if (M <= 0 || MaxN <= 0) return;
                double fourier_even = list_an[0] / 2.0;
                double fourier_odd = 0;
                for (int i = 0; i < maxN; i++)
                {
                    if(i > 0) fourier_even += list_an[i] * Math.Cos((i * Math.PI * x) / d);
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
                count++;
            }
            double[] criticalPoints = { a, d };
            foreach (var point in criticalPoints)
            {
                if (TempSum.Find(x => x.X == point) == null)
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
            TempSum.Sort((item1, item2) => item1.X.CompareTo(item2.X)); //Соритировка по числам (х) по возрастанию
            for (int i = 0; i < TempSum.Count; i++) //Восстановление индексов
            {
                TempSum[i].Id = i + 1;
            }
            TableSum = TempSum;
            var dynamicPoints_an = new ObservableCollection<ObservablePoint>();
            var dynamicPoints_bn = new ObservableCollection<ObservablePoint>();
            foreach (var item in TableSum)
            {
                dynamicPoints_an.Add(new ObservablePoint(item.X, item.SumEven));
                dynamicPoints_bn.Add(new ObservablePoint(item.X, item.SumOdd));
            }
            MySeries = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = staticPoints,
                    Name = "Эталонная f(x)",
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Gray, 2) // Серая линия
                },
                new LineSeries<ObservablePoint>
                {
                    Values = dynamicPoints_an,
                    Name = "Четная f(x)",
                    GeometrySize = 0, // Убираем маркеры точек, оставляем гладкую линию
                    Fill = null, // Отключаем заливку под графиком 
                    Stroke = new SolidColorPaint(SKColors.Green, 2)
                },
                 new LineSeries<ObservablePoint>
                {
                    Values = dynamicPoints_bn,
                    Name = "Нечетная f(x)",
                    GeometrySize = 0, // Убираем маркеры точек, оставляем гладкую линию
                    Fill = null, // Отключаем заливку под графиком 
                    Stroke = new SolidColorPaint(SKColors.Purple, 2)
                }
            };
        }
        public double SumFunctionTgCos(int j, double step)
        {
            double sum = 0;
            for (int i = 1; i <= M - 1; i++)
            {
                double x = step * i;
                sum += ReturnZnachFuncTgCos(x, j);
            }
            return sum;
        }
        public double ReturnZnachFuncTgCos(double x, int i)
        {
            return Math.Tan(x) * Math.Cos((i * Math.PI * x) / d);
        }
        public double SumFunctionTgSin(int j, double step)
        {
            double sum = 0;
            for (int i = 1; i <= M - 1; i++)
            {
                double x = step * i;
                sum += ReturnZnachFuncTgSin(x, j);
            }
            return sum;
        }
        public double ReturnZnachFuncTgSin(double x, int i)
        {
            return Math.Tan(x) * Math.Sin((i * Math.PI * x) / d);
        }
        public void InFunctionSeries()
        {
            MainWindowViewModel.Self.FunctionSeriesVM = new FunctionSeriesViewModel();
            MainWindowViewModel.Self.Page = new FunctionSeriesView();

        }
    }
}
