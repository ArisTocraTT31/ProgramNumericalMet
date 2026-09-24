using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProgramNumericalMet.ViewModels;

namespace ProgramNumericalMet.Views;

public partial class SolvingEquationsViews : UserControl
{
    public SolvingEquationsViews()
    {
        InitializeComponent();
        DataContext = new SolvingEquationsViewsModel();
    }
}