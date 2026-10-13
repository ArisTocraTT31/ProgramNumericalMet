using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.ViewModels
{
    public class DefinedIntegralsViewModel : ViewModelBase
    {
        double _a = 0;
        double _b = Math.PI / 2;
        int maxN_n1 = 10;
        int maxN_n2 = 10;
        double itog_n1;
        double itog_n2;
        double _d = Math.Pow(10, -6);
        int counter_n1;
        int counter_n2;

        public double a { get => _a; set => _a = value; }
        public double b { get => _b; set => _b = value; }
        public int Counter_n1 { get => counter_n1; set => this.RaiseAndSetIfChanged(ref counter_n1, value); }
        public int Counter_n2 { get => counter_n2; set => this.RaiseAndSetIfChanged(ref counter_n2, value); }
        public int MaxN_n1 { get => maxN_n1; set => this.RaiseAndSetIfChanged(ref maxN_n1, value); }
        public double Itog_n1 { get => itog_n1; set => this.RaiseAndSetIfChanged(ref itog_n1, value); }
        public int MaxN_n2 { get => maxN_n2; set => this.RaiseAndSetIfChanged(ref maxN_n2, value); }
        public double Itog_n2 { get => itog_n2; set => this.RaiseAndSetIfChanged(ref itog_n2, value); }
        public double d { get => _d; set => _d = value; }

        public DefinedIntegralsViewModel()
        {

        }

        public void ButtonAction_n1()
        {
            Counter_n1 = 0;
            Itog_n1 = ReturnItog1(MaxN_n1);
            double delta = 0;
            int n = MaxN_n1;
            while (true)
            {
                delta = (1 / 15) * Math.Abs(ReturnItog1(2 * n) - ReturnItog1(n));
                if (delta < d) return;
                else n = n * 2;
            }
        }
        public void ButtonAction_n2()
        {
            if (MaxN_n2 % 2 != 0) return;
            Counter_n2 = 0;
            Itog_n2 = ReturnItog2(MaxN_n2);
            double delta = 0;
            int n = MaxN_n2;
            while (true)
            {
                delta = (1 / 15) * Math.Abs(ReturnItog2(2 * n) - ReturnItog2(n));
                if (delta < d) return;
                else n = n * 2;
            }

        }
        public double ReturnFunc(double x, int method)
        {
            if(method == 1) Counter_n1++;
            else Counter_n2++;
            return Math.Pow(Math.Cos(x), 2);
        }
        public double ReturnItog1(double n)
        {
            double h = (b - a) / n;
            double sum_func = 0;
            for (int i = 1; i < n; i++)
            {
                double x = i * h;
                sum_func += ReturnFunc(x,1);
            }
            return h * (((ReturnFunc(a,1) + ReturnFunc(b, 1)) / 2) + sum_func);
        }
        public double ReturnItog2(double n)
        {
            double h = (b - a) / n;
            double sum_func_even = 0;
            double sum_func_odd = 0;
            for (int i = 1; i <= n / 2; i++)
            {
                if (i >= 2) sum_func_odd += ReturnFunc(a + h * (2 * i - 2),2);
                sum_func_even += ReturnFunc(a + h * (2 * i - 1),2);
            }
            return (h / 3) * (ReturnFunc(a,2) + ReturnFunc(b,2) + 4 * sum_func_even + 2 * sum_func_odd);
        }
        
    }
}
