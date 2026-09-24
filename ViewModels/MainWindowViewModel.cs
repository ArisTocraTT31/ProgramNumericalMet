using Avalonia.Controls;
using ReactiveUI;
using ProgramNumericalMet.Views;
using ProgramNumericalMet.Models;
using System.Collections.Generic;

namespace ProgramNumericalMet.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        UserControl uc = new Markup();
        UserControl page = new MainScreen();
        MainScreenViewModel mainScreenViewModel;
        FunctionSeriesViewModel functionSeriesViewModel;
        FourierSeriesViewModel fourierSeriesView;
        SolvingEquationsViewsModel solvingEquationsViewsModel;

        public UserControl UC { get => uc; set => this.RaiseAndSetIfChanged(ref uc, value); }
        public UserControl Page { get => page;set=>this.RaiseAndSetIfChanged(ref page, value); }
        public MainScreenViewModel MainScreenVM { get => mainScreenViewModel; set=>mainScreenViewModel = value; }
        public FunctionSeriesViewModel FunctionSeriesVM { get => functionSeriesViewModel; set => functionSeriesViewModel = value; }
        public FourierSeriesViewModel FourierSeriesVM { get => fourierSeriesView; set => fourierSeriesView = value; }
        public SolvingEquationsViewsModel SolvingEquationsVM { get => solvingEquationsViewsModel; set => solvingEquationsViewsModel = value; }


        public static MainWindowViewModel Self;
        public MainWindowViewModel()
        {
            Self = this;
           
            MainScreenVM = new MainScreenViewModel();
        }
    }
}
