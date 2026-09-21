using ReactiveUI;
using System;
using System.Collections.Generic;
using ProgramNumericalMet.Views;
using ProgramNumericalMet.Models;
using Avalonia.Controls;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Avalonia.Media;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ProgramNumericalMet.ViewModels
{
    public class FunctionSeriesViewModel : ViewModelBase
    {

        int _a = 0;
        double _b = 1.4;
        int step; //Шаг
        int maxN; //Кол-во n для суммы
        List<FSValue> tableSum; //Лист значений 

        private ObservableCollection<ObservablePoint> staticPoints;

        public int a { get => _a; set => _a = value; }
        public double b { get => _b; set => _b = value; }
        public int Step { get => step; set => this.RaiseAndSetIfChanged(ref step, value); }
        public int MaxN { get => maxN; set => this.RaiseAndSetIfChanged(ref maxN, value); }
        public List<FSValue> TableSum { get => tableSum; set => this.RaiseAndSetIfChanged(ref tableSum, value); }


        public Axis[] XAxes { get; set; }
        public ISeries[] mySeries;
        public ISeries[] MySeries { get => mySeries; set => this.RaiseAndSetIfChanged(ref mySeries, value); }

        public FunctionSeriesViewModel()
        {
            MySeries = Array.Empty<ISeries>();
            staticPoints = new ObservableCollection<ObservablePoint>();
            double minX = a - b; //Минимальное значение на отрезке
            double maxX = a + b; //Максимальное значение на отрезке
            XAxes = new Axis[]
            {
                new Axis
                {
                    MinLimit = -1.4,
                    MaxLimit = 1.4,
                }
            };
            for (double x = minX; x <= 10; x += 0.01)
            {
                if (Math.Abs((x * x) - 2) < 0.0001) continue;
                double y = x / ((x * x) - 2);
                staticPoints.Add(new ObservablePoint(x, y));
            }
            MySeries = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = staticPoints,
                    Name = "f(x)",
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Gray,2)
                }
            };
        }

        //-x^(2n+1)
        //--------
        // 2^(n+1)

        public void ButtonAction() //Метод, который активируется по кнопке, он делает рассчеты суммы для каждого элемента с точностью n и с конкретным шагом на заданном отрезке
        {
            if (Step <= 0 || MaxN < 0) return;
            List<FSValue> TempSum = new List<FSValue>(); //Временный лист данных
            double minX = a - b; //Минимальное значение на отрезке
            double maxX = a + b; //Максимальное значение на отрезке
            double[] criticalPoints = { 0, minX, maxX }; //Массив критических точек (которые обязательно должны быть в итоговых расчетах)
            /*int n = (int)Math.Round(((maxX - minX) / Step) + 1);*/ //Узнаем кол-во элементов от заданного шага
            double n = Math.Round((maxX - minX) / Step,6);
            for (double x = minX; x < maxX; x = Math.Round((x+n),6))
            {
                /*double x = Math.Round(((a - b) + Step * i), 6, MidpointRounding.AwayFromZero);*/ //Расчет х, начиная с минимума, идя по шагу до максимума
                if (x > (a + b) || x < (a - b)) break; //Прекращение работы цикла, если х будет выходить за рамки отрезка
                double y = 0;
                double sum = 0;
                double tern = 0;
                double multiplier = (x * x) / 2.0;
                for (int j = 0; j <= MaxN; j++) //Цикл для вычисление суммы ряда для х за введенное кол-во n
                {
                    if (j == 0) //Первый элемент ряда
                    {
                        tern = -x / 2.0;
                        sum = tern;
                    }
                    else
                    {
                        //(каждый последующий член) * (x^2/2)
                        tern = tern * multiplier;
                        sum = sum + tern; //Суммируются все элементы ряда
                    }
                    y = x / ((x * x) - 2); //Вычисление у по изначальной функции
                }
                TempSum.Add(new FSValue //Добавление элемента в лист
                {
                    Id = 1 + 1,
                    X = x,
                    Y = Math.Round(y, 6),
                    Sum = Math.Round(sum, 10)
                });
            }
            foreach (var point in criticalPoints) //Цикл проверки
            {
                if (TempSum.Find(x => x.X == point) == null) //Если в листе нет критических точек, то они будут добавляться и рассчитываться отдельно
                {
                    double x = point;
                    double y = 0;
                    double sum = 0;
                    double tern = 0;
                    double multiplier = (x * x) / 2.0;
                    for (int j = 0; j <= MaxN; j++)
                    {
                        if (j == 0)
                        {
                            tern = -x / 2.0;
                            sum = tern;
                        }
                        else
                        {
                            tern = tern * multiplier;
                            sum = sum + tern;
                        }
                        y = x / ((x * x) - 2);
                    }
                    TempSum.Add(new FSValue
                    {
                        Id = 0, //Временный индекс
                        X = x,
                        Y = Math.Round(y, 6),
                        Sum = Math.Round(sum, 10)
                    });
                }
            }
            TempSum.Sort((item1, item2) => item1.X.CompareTo(item2.X)); //Соритировка по числам (х) по возрастанию
            for (int i = 0; i < TempSum.Count; i++) //Восстановление индексов
            {
                TempSum[i].Id = i + 1;
            }
            TableSum = TempSum;

            var dynamicPoints = new ObservableCollection<ObservablePoint>();
            foreach (var item in TableSum)
            {
                // Защита от точек разрыва функции (ОДЗ: x^2 != 2), чтобы график не улетал в бесконечность
                if (double.IsInfinity(item.Y) || double.IsNaN(item.Y) || Math.Abs((item.X * item.X) - 2) < 0.0001) continue;
                dynamicPoints.Add(new ObservablePoint(item.X, item.Sum));
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
                    Values = dynamicPoints,
                    Name = "Значение f(x)",
                    GeometrySize = 0, // Убираем маркеры точек, оставляем гладкую линию
                    Fill = null // Отключаем заливку под графиком 
                }
            };
        }
        public void InFourierSeries()
        {
            MainWindowViewModel.Self.FourierSeriesVM = new FourierSeriesViewModel();
            MainWindowViewModel.Self.Page = new FourierSeriesView();
        }
    }
}
