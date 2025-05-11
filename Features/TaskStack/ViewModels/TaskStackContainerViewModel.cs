using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using TaskStack.Data;
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
            var task = await _context.Tasks.SingleAsync(task => task.Id == message.Value);
            SelectedTaskStack = new TaskStackViewModel(_context)
            {
                TaskId = task.Id,
                Title = task.Title,
                Tasks = new ObservableCollection<string>(task.Tasks)
            };
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
}