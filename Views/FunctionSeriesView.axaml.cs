using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProgramNumericalMet.ViewModels;

namespace ProgramNumericalMet.Views;

public partial class FunctionSeriesView : UserControl
{
    public FunctionSeriesView()
    {
        InitializeComponent();
        DataContext = new FunctionSeriesViewModel();
    }
}