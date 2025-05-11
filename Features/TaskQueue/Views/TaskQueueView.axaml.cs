using Avalonia.Controls;
using TaskStack.Features.TaskQueue.ViewModels;

namespace TaskStack.Features.TaskQueue.Views;

public partial class TaskQueueView : UserControl
{
    public TaskQueueView()
    {
        InitializeComponent();
        DataContext = new TaskQueueViewModel();
    }
}