using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
