using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using TaskStack.Features.TaskQueue.ViewModels;

namespace TaskStack.Features.TaskQueue.Views;

public partial class TaskEditView : UserControl
{
    public TaskEditView()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        TitleTextBox.Focus();
    }

    private void TitleTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        // Ignore if the task has been initialized.
        if (DataContext is TaskViewModel { Initialised: true }) return;
        
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
}