using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Data;
using System.Collections.Generic;
using System.Linq;
using ProgramNumericalMet.ViewModels;
using ReactiveUI;
using System.ComponentModel;


namespace ProgramNumericalMet.Views;

public partial class ApproximationView : UserControl
{
    public ApproximationView()
    {
        InitializeComponent();

        var viewModel = new ApproximationViewModel();
        DataContext = viewModel;

        viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (DataContext is not ApproximationViewModel vm) return;

        // 1. Обработка матрицы Matrix_A
        if (e.PropertyName == nameof(ApproximationViewModel.Matrix_A))
        {
            MatrixDataGrid.ItemsSource = null;
            if (vm.Matrix_A != null && vm.Matrix_A.Count > 0)
            {
                MatrixDataGrid.Columns.Clear();
                for (int i = 0; i < vm.Matrix_A.Count; i++)
                {
                    MatrixDataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = $"j = {i}",
                        Binding = new Binding($"[{i}]"),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                    });
                }
                MatrixDataGrid.ItemsSource = vm.Matrix_A;
            }
        }

        // 2. Обработка списка Array_X
        if (e.PropertyName == nameof(ApproximationViewModel.Array_X))
        {
            GridX.ItemsSource = null;
            if (vm.Array_X != null)
            {
                GridX.Columns.Clear();
                GridX.Columns.Add(new DataGridTextColumn
                {
                    Header = "Значение X",
                    Binding = new Binding("."), // "." означает привязку к самому элементу double напрямую
                    Width = new DataGridLength(1, GridX.Columns.Count == 0 ? DataGridLengthUnitType.Star : DataGridLengthUnitType.SizeToCells)
                });
                GridX.ItemsSource = vm.Array_X;
            }
        }

        // 3. Обработка списка List_func
        if (e.PropertyName == nameof(ApproximationViewModel.List_func))
        {
            GridFunc.ItemsSource = null;
            if (vm.List_func != null)
            {
                GridFunc.Columns.Clear();
                GridFunc.Columns.Add(new DataGridTextColumn
                {
                    Header = "Значение F(x)",
                    Binding = new Binding("."),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                });
                GridFunc.ItemsSource = vm.List_func;
            }
        }

        // 4. Обработка списка Array_B
        if (e.PropertyName == nameof(ApproximationViewModel.Array_B))
        {
            GridB.ItemsSource = null;
            if (vm.Array_B != null)
            {
                GridB.Columns.Clear();
                GridB.Columns.Add(new DataGridTextColumn
                {
                    Header = "Элемент B",
                    Binding = new Binding("."),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                });
                GridB.ItemsSource = vm.Array_B;
            }
        }

        // 5. Обработка списка Array_C (Коэффициенты многочлена)
        if (e.PropertyName == nameof(ApproximationViewModel.Array_C))
        {
            GridC.ItemsSource = null;
            if (vm.Array_C != null)
            {
                GridC.Columns.Clear();
                GridC.Columns.Add(new DataGridTextColumn
                {
                    Header = "Коэф. C",
                    Binding = new Binding("."),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                });
                GridC.ItemsSource = vm.Array_C;
            }
        }
    }
}
