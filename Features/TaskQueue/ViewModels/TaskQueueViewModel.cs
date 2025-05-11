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

public partial class TaskQueueViewModel : ObservableObject, IRecipient<TaskDeletedMessage>
{
    private readonly TaskContext _context = Ioc.Default.GetRequiredService<TaskContext>();

    [ObservableProperty] private ObservableCollection<TaskViewModel> _tasks;

    [ObservableProperty] private TaskViewModel? _selectedTask;
    
    [ObservableProperty] private TaskViewModel? _newTask;

    public bool CanAddTask => NewTask is null;

    public TaskQueueViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
        _tasks = InitializeTaskCollection();
    }

    private ObservableCollection<TaskViewModel> InitializeTaskCollection()
    {
        return new ObservableCollection<TaskViewModel>(
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
        WeakReferenceMessenger.Default.Send(new TaskSelectedMessage(value?.Id));
    }

    [RelayCommand(CanExecute = nameof(CanAddTask))]
    private Task AddTaskAsync()
    {
        NewTask = new TaskViewModel();
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task TaskCreated(TaskViewModel task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            AddTaskCommand.NotifyCanExecuteChanged();
            return;
        }

        await PersistCreatedTask(task);

        Tasks.Insert(0, task);
        NewTask = null;

        AddTaskCommand.NotifyCanExecuteChanged();
        SelectedTask = task;
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

    public void Receive(TaskDeletedMessage message)
    {
        var task = Tasks.SingleOrDefault(task => task.Id == message.Value);

        if (task is null) return;

        Tasks.Remove(task);
    }
}