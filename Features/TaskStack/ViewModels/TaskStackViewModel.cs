using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using TaskStack.Data;
using TaskStack.Data.Models;
using TaskStack.Features.TaskStack.Models;

namespace TaskStack.Features.TaskStack.ViewModels;

public partial class TaskStackViewModel(TaskContext context) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<TaskStep> _tasks = [];
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _newStep = string.Empty;
    

    public bool CanPopStep => Tasks.Count != 0;

    public int TaskId { get; init; }

    [RelayCommand]
    private async Task NewStepKeyDown(KeyEventArgs args)
    {
        if (args.Key != Key.Enter) return;
        
        var task = await context.Tasks.SingleAsync(task => task.Id == TaskId);

        var taskStepEntity = new TaskStepEntity()
        {
            Step = NewStep,
            Created = DateTime.UtcNow,
        };
        
        task.Tasks.Add(taskStepEntity);
        
        await context.SaveChangesAsync();
        
        Tasks.Insert(0, new TaskStep()
        {
            Id = taskStepEntity.Id,
            Step = taskStepEntity.Step,
        });
        
        NewStep = string.Empty;
        PopTaskStepCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanPopStep))]
    private async Task PopTaskStepAsync()
    {
        if(Tasks.Count == 0) return;

        var task = await context.Tasks.Include(t => t.Tasks).SingleAsync(task => task.Id == TaskId);
        var topStackStep = task.Tasks.Where(t => t.Completed == null).OrderByDescending(t => t.Created).First();
        topStackStep.Completed = DateTime.UtcNow;
        
        context.Entry(topStackStep).State = EntityState.Modified;
        await context.SaveChangesAsync();
        
        Tasks.RemoveAt(0);
        
        PopTaskStepCommand.NotifyCanExecuteChanged();
    }
}