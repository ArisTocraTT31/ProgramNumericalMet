using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProgramNumericalMet.ViewModels;

namespace ProgramNumericalMet.Views;

public partial class DefinedIntegralsView : UserControl
{
    public DefinedIntegralsView()
    {
        InitializeComponent();
        DataContext = new DefinedIntegralsViewModel();
    }
}