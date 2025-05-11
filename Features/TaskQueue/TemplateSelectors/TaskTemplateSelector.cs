using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using TaskStack.Features.TaskQueue.ViewModels;

namespace TaskStack.Features.TaskQueue.TemplateSelectors;

public class TaskTemplateSelector : IDataTemplate
{
    [Content]
    public IDataTemplate? DefaultTemplate { get; set; }
    
    public IDataTemplate? EditTemplate { get; set; }
    
    public Control? Build(object? param)
    {
        if(param is TaskViewModel { Initialised: true} viewModel)
        {
            return DefaultTemplate?.Build(viewModel);
        }
        
        return EditTemplate?.Build(param);
    }

    public bool Match(object? data)
    {
        return data is TaskViewModel;
    }
}