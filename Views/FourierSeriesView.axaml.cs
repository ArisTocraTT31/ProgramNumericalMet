using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProgramNumericalMet.ViewModels;

namespace ProgramNumericalMet.Views;

public partial class FourierSeriesView : UserControl
{
    public FourierSeriesView()
    {
        InitializeComponent();
        DataContext = new FourierSeriesViewModel();
    }
}