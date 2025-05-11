using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskStack.Data;
using TaskStack.Messages;

namespace TaskStack.Features.TaskStack.ViewModels;

public partial class TaskStackViewModel(TaskContext context) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> _tasks = [];
    [ObservableProperty] private string _title = string.Empty;

    public int TaskId { get; init; }

}