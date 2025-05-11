using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TaskStack.Data;

namespace TaskStack.Features.TaskStack.ViewModels;

public partial class TaskStackViewModel(TaskContext? context = null) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> _tasks = [];
    [ObservableProperty] private string _title = string.Empty;

    public int TaskId { get; init; }

}