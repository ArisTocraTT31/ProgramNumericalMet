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
    public class MainScreenViewModel : ViewModelBase
    {
        int id;
        Zadania? selectedChoice;
        List<Zadania> listZadania;

        public int Id { get => id;set=>this.RaiseAndSetIfChanged(ref id, value); }
        public Zadania? SelectedChoice { get => selectedChoice; set { 
                this.RaiseAndSetIfChanged(ref selectedChoice, value);
                if (value != null)
                {
                    Id = value.Id;
                    ChoiseZadanie(Id);
                }
            } 
        }
        public List<Zadania> ListZadania { get => listZadania; set => this.RaiseAndSetIfChanged(ref listZadania, value); }

        public MainScreenViewModel()
        {
            ListZadania = new List<Zadania>(Zadania.All);
        }
        public void ChoiseZadanie(int Id_Zadanie)
        {
            switch (Id_Zadanie)
            {
                case 1:
                    MainWindowViewModel.Self.FunctionSeriesVM = new FunctionSeriesViewModel();
                    MainWindowViewModel.Self.Page = new FunctionSeriesView();
                    break;
                case 2:
                    MainWindowViewModel.Self.SolvingEquationsVM = new SolvingEquationsViewsModel();
                    MainWindowViewModel.Self.Page = new SolvingEquationsViews();
                    break;
                default:
                    MainWindowViewModel.Self.Page = new MainScreen();
                    break;
            }
        }
        public void ExitChoice()
        {
            MainWindowViewModel.Self.Page = new MainScreen();
        }
    }
}
