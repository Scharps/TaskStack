using Avalonia.Controls;
using TaskStack.Features.TaskStack.ViewModels;

namespace TaskStack.Features.TaskStack.Views;

public partial class TaskStackContainerView : UserControl
{
    public TaskStackContainerView()
    {
        InitializeComponent();
        DataContext = new TaskStackContainerViewModel();
    }
}