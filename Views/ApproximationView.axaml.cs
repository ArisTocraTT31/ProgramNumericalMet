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

        // 1. Создаем и привязываем ViewModel
        var viewModel = new ApproximationViewModel();
        DataContext = viewModel;

        // 2. Подписываемся на стандартное событие изменения свойств
        viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ApproximationViewModel.Matrix_A))
        {
            if (DataContext is ApproximationViewModel vm)
            {
                var matrix = vm.Matrix_A;

                // СБРАСЫВАЕМ ИСТОЧНИК: говорим DataGrid временно забыть про данные
                MatrixDataGrid.ItemsSource = null;

                if (matrix == null || matrix.Count == 0)
                {
                    MatrixDataGrid.Columns.Clear();
                    return;
                }

                // Очищаем и создаем колонки заново
                MatrixDataGrid.Columns.Clear();
                int columnsCount = matrix.Count;

                for (int i = 0; i < columnsCount; i++)
                {
                    MatrixDataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = $"i = {i}",
                        Binding = new Binding($"[{i}]"),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                    });
                }

                // НАЗНАЧАЕМ ИСТОЧНИК ЗАНОВО: теперь строки создадутся строго по готовым колонкам
                MatrixDataGrid.ItemsSource = matrix;
            }
        }
    }
}
