using ProgramNumericalMet.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.ViewModels
{
    public class FourierSeriesViewModel : ViewModelBase
    {


        public void InFunctionSeries()
        {
            MainWindowViewModel.Self.FunctionSeriesVM = new FunctionSeriesViewModel();
            MainWindowViewModel.Self.Page = new FunctionSeriesView();
        }
    }
}
