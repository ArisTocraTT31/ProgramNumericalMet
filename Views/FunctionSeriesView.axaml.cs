using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ProgramNumericalMet.ViewModels;

namespace ProgramNumericalMet.Views;

public partial class FunctionSeriesView : UserControl
{
    public FunctionSeriesView()
    {
        InitializeComponent();
        DataContext = new FunctionSeriesViewModel();
        LiveCharts.Configure(config => config.HasGlobalSKTypeface(SkiaSharp.SKTypeface.FromFamilyName("Arial")));
    }
}