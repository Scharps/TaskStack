using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using TaskStack.Data;
using TaskStack.Features.TaskStack.Models;
using TaskStack.Messages;

namespace TaskStack.Features.TaskStack.ViewModels;

public partial class TaskStackContainerViewModel : ObservableObject, IRecipient<TaskSelectedMessage>
{
    private readonly TaskContext _context;
    
    [ObservableProperty]
    private TaskStackViewModel? _selectedTaskStack;
    
    public TaskStackContainerViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
        _context = Ioc.Default.GetRequiredService<TaskContext>();
    }
    
    public async void Receive(TaskSelectedMessage message)
    {
        try
        {
            var task = await _context.Tasks.Include(taskEntity => taskEntity.Tasks).SingleOrDefaultAsync(task => task.Id == message.Value);

            SelectedTaskStack = task switch
            {
                not null => new TaskStackViewModel(_context)
                {
                    TaskId = task.Id,
                    Title = task.Title,
                    Tasks = new ObservableCollection<TaskStep>(task.Tasks.OrderByDescending(t => t.Created).Select(t => new TaskStep()
                    {
                        Id = t.Id,
                        Step = t.Step
                    }))
                },
                _ => null,
            };
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
    
    [RelayCommand]
    private async Task DeleteTaskAsync()
    {
        var taskEntity = await _context.Tasks.FindAsync(SelectedTaskStack!.TaskId);
        if (taskEntity is null) return;
        _context.Tasks.Remove(taskEntity);
        if (await _context.SaveChangesAsync() > 0)
        {
            WeakReferenceMessenger.Default.Send(new TaskDeletedMessage(SelectedTaskStack.TaskId));
        }
    }
}