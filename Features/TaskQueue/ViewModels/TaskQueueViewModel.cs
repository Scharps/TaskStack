using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskStack.Data;
using TaskStack.Data.Models;
using TaskStack.Messages;

namespace TaskStack.Features.TaskQueue.ViewModels;

public partial class TaskQueueViewModel : ObservableObject
{
    private readonly TaskContext _context = Ioc.Default.GetRequiredService<TaskContext>();

    [ObservableProperty] private ObservableCollection<TaskViewModel> _tasks;

    [ObservableProperty] private TaskViewModel? _selectedTask;

    public bool CanAddTask => Tasks.Count == 0 || Tasks[0].Initialised;

    public TaskQueueViewModel()
    {
        _tasks = new ObservableCollection<TaskViewModel>(
            _context.Tasks
                .OrderByDescending(t => t.Created)
                .Select(s =>
            new TaskViewModel
            {
                Initialised = true,
                Id = s.Id,
                Title = s.Title,
            }));
    }

    partial void OnSelectedTaskChanged(TaskViewModel? value)
    {
        // User entering space on the edit causes selection to change. Check initialization to be sure.
        if (value is not { Initialised: true }) return;
        
        WeakReferenceMessenger.Default.Send(new TaskSelectedMessage(value!.Id));
    }

    [RelayCommand(CanExecute = nameof(CanAddTask))]
    private Task AddTaskAsync()
    {
        Tasks.Insert(0, new TaskViewModel());
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task TaskInitialised(TaskViewModel task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            Tasks.RemoveAt(0);
            AddTaskCommand.NotifyCanExecuteChanged();
            return;
        }

        await PersistCreatedTask(task);

        // Set to trigger collection update
        Tasks[0] = task;

        AddTaskCommand.NotifyCanExecuteChanged();
    }

    private async Task PersistCreatedTask(TaskViewModel task)
    {
        var taskEntity = new TaskEntity()
        {
            Title = task.Title!,
            Created = DateTime.UtcNow
        };

        _context.Tasks.Add(taskEntity);

        await _context.SaveChangesAsync();

        task.Id = taskEntity.Id;
        task.Initialised = true;
    }
}