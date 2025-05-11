using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskStack.Features.TaskQueue.ViewModels;

public partial class TaskViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _title;
    [ObservableProperty]
    private bool _initialised;
    [ObservableProperty]
    private int _id;
}
