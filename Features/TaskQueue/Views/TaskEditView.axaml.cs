using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace TaskStack.Features.TaskQueue.Views;

public partial class TaskEditView : UserControl
{
    public TaskEditView()
    {
        InitializeComponent();
    }

    private void TitleTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (DataContext is null) return;
        
        SaveCommand?.Execute(DataContext);
    }

    private void TitleTextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SaveCommand?.Execute(DataContext);        
        }
    }

    public static readonly StyledProperty<ICommand?> SaveCommandProperty =
        AvaloniaProperty.Register<TaskEditView, ICommand?>(nameof(SaveCommand));
    
    public ICommand? SaveCommand
    {
        get => GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    private void StyledElement_OnDataContextChanged(object? sender, EventArgs e)
    {
        if(DataContext is null) return;

        TitleTextBox.Focus();
    }
}