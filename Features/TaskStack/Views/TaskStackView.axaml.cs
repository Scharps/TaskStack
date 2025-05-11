using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using TaskStack.Features.TaskStack.ViewModels;

namespace TaskStack.Features.TaskStack.Views;

public partial class TaskStackView : UserControl
{
    public TaskStackView()
    {
        InitializeComponent();
    }
}