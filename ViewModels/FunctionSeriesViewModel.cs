using ReactiveUI;
using System;
using System.Collections.Generic;
using ProgramNumericalMet.Views;
using Avalonia.Controls;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.ViewModels
{
    public class FunctionSeriesViewModel : ViewModelBase
    {
        public class FSValue
        {
            public int Id { get; set; }
            public double X { get; set; }
            public double Sum { get; set; }
        }

        int _a = 0;
        double _b = 1.4;
        double step;
        int maxN;
        List<FSValue> tableSum;

        public int a { get => _a; set => _a = value; }
        public double b { get => _b; set => _b = value; }
        public double Step { get => step; set => this.RaiseAndSetIfChanged(ref step, value); }
        public int MaxN { get => maxN; set => this.RaiseAndSetIfChanged(ref maxN, value); }
        public List<FSValue> TableSum { get => tableSum; set => this.RaiseAndSetIfChanged(ref tableSum, value); }

        //-x^(2n+1)
        //--------
        // 2^(n+1)

        public FunctionSeriesViewModel()
        {

        }
        public void ButtonAction()
        {
            List<FSValue> TempSum = new List<FSValue>();
            int n = (int)Math.Round((((a + b) - (a - b)) / Step) + 1);
            for (int i = 0; i < n; i++)
            {
                double x = Math.Round(((a - b) + Step * i), 2, MidpointRounding.AwayFromZero);
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
                }
                TempSum.Add(new FSValue
                {
                    Id = i+1,
                    X = x,
                    Sum = Math.Round(sum,6)
                });
            }
            TableSum = TempSum;
        }
    }
}
