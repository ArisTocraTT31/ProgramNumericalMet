using ProgramNumericalMet.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.ViewModels
{
    public class SolvingEquationsViewsModel : ViewModelBase
    {
        double _a1;
        double _b1;
        double _a2 = -3;
        double _b2 = 0;
        double itog_x1;
        List<SolEqua> list_SolEquas_n1;
        List<SolEqua> list_SolEquas_n2;

        public double a1 { get => _a1; set => this.RaiseAndSetIfChanged(ref _a1, value); }
        public double b1 { get => _b1; set => this.RaiseAndSetIfChanged(ref _b1, value); }
        public double a2 { get => _a2; set => this.RaiseAndSetIfChanged(ref _a2, value); }
        public double b2 { get => _b2; set => this.RaiseAndSetIfChanged(ref _b2, value); }
        public double Itog_x1 { get => itog_x1; set => this.RaiseAndSetIfChanged(ref itog_x1, value); }
        public List<SolEqua> List_SolEquas_n1 { get => list_SolEquas_n1; set => this.RaiseAndSetIfChanged(ref list_SolEquas_n1, value); }
        public List<SolEqua> List_SolEquas_n2 { get => list_SolEquas_n2; set => this.RaiseAndSetIfChanged(ref list_SolEquas_n2, value); }

        public SolvingEquationsViewsModel()
        {

        }
        public void ButtonAction_n1()
        {
            if (a1 > b1) return;
            double temp_a = a1;
            double temp_b = b1;
            double d = Math.Pow(10, -11);
            List<SolEqua> temp_list = new List<SolEqua>();
            int i = 0;
            while ((temp_b - temp_a) > d)
            {
                double x = (temp_a + temp_b) / 2;
                temp_list.Add(new SolEqua
                {
                    Id = i + 1,
                    A = temp_a,
                    B = temp_b,
                    X = x,
                    Func = FunctionValue(x)
                });
                if (FunctionValue(temp_a) * FunctionValue(x) < 0) temp_b = x;
                else temp_a = x;
                Itog_x1 = x;
                i++;
            }
            List_SolEquas_n1 = temp_list;
        }
        public void ButtonAction_n2()
        {
            if (a2 > b2) return;
            double d = Math.Pow(10, -11);
            List<SolEqua> temp_list = new List<SolEqua>();
            double x0 = 0;
            if (FunctionValue(a2) * FunctionValue_2(a2) > 0) x0 = a2;
            else if (FunctionValue(b2) * FunctionValue_2(b2) > 0) x0 = b2;
            double temp_x = x0 - FunctionValue(x0) / FunctionValue_1(x0);
            temp_list.Add(new SolEqua
            {
                Id = 1,
                X = temp_x,
                A = FunctionValue(temp_x),
                B = FunctionValue_1(temp_x)
            });
            int i = 1;
            //Math.Abs(temp_list[i].X - temp_list[i - 1].X) > d
            while (i<8)
            {
                double x = temp_list[i - 1].X - FunctionValue(temp_list[i - 1].X) / FunctionValue_1(temp_list[i - 1].X);
                temp_list.Add(new SolEqua
                {
                    Id = i + 1,
                    X = x,
                    A = FunctionValue(x),
                    B = FunctionValue_1(x)
                });
                i++;
            }
            List_SolEquas_n2 = temp_list;
        }
        public double FunctionValue(double x)
        {
            return 0.88 * Math.Pow(x, 3) - 2.81 * Math.Pow(x, 2) - 3.692 * x + 13.1;
        }
        public double FunctionValue_1(double x)
        {
            return 2.64 * Math.Pow(x, 2) - 5.62 * x - 3.692;
        }
        public double FunctionValue_2(double x)
        {
            return 5.28 * x - 5.62;
        }
    }
}
