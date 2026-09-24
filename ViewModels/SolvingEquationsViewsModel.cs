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
        int _a1;
        int _b1;
        int _a2;
        int _b2;

        public int a1 { get => _a1; set => this.RaiseAndSetIfChanged(ref _a1, value); }
        public int b1 { get => _b1; set => this.RaiseAndSetIfChanged(ref _b1, value); }
        public int a2 { get => _a2; set => this.RaiseAndSetIfChanged(ref _a2, value); }
        public int b2 { get => _b2; set => this.RaiseAndSetIfChanged(ref _b2, value); }
    }
}
