using ProgramNumericalMet.Models;
using ProgramNumericalMet.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.ViewModels
{
    public class FourierSeriesViewModel : ViewModelBase
    {
        int _a = 0;
        double _d = 1.5;
        int maxN;
        int m; //Шаг
        List<FSValue> tableSum;


        public int a { get => _a; set => _a = value; }
        public double d { get => _d; set => _d = value; }
        public int MaxN { get => maxN; set => this.RaiseAndSetIfChanged(ref maxN, value); }
        public int M { get => m; set => this.RaiseAndSetIfChanged(ref m, value); }
        public List<FSValue> TableSum { get => tableSum; set => this.RaiseAndSetIfChanged(ref tableSum, value); }

        public FourierSeriesViewModel()
        {

        }

        public void ButtonAction()
        {
            List<FSValue> TempSum = new List<FSValue>();
            double a0 = Math.Round((-2/d)*Math.Log(Math.Abs(Math.Cos(d))),3);
            double n = Math.Round(d / M,6);
            for (double x = a; x < d; x = Math.Round((x + n), 6))
            {
                for (int j = 1; j < maxN; j++)
                {
                    double an = (2 / d) * d / M * (((ReturnZnachFuncTgCos(a,0) + ReturnZnachFuncTgCos(d, M))/2)+SumFunctionTgCos(j, n));
                }
            }
        }
        public double SumFunctionTgCos(double n, double step)
        {
            double sum = 0;
            for(double i = 1; i< M -1; i++)
            {
                double x = Math.Round((step * i), 6, MidpointRounding.AwayFromZero);
                sum += ReturnZnachFuncTgCos(x, n);
            }
            return sum;
        }
        public double ReturnZnachFuncTgCos(double x, double n)
        {
            return Math.Round(Math.Tan(x) * Math.Cos((n * Math.PI * x) / d), 6);
        }

        public void InFunctionSeries()
        {
            MainWindowViewModel.Self.FunctionSeriesVM = new FunctionSeriesViewModel();
            MainWindowViewModel.Self.Page = new FunctionSeriesView();
           
        }
    }
}
