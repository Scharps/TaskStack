using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskStack.Data;
using TaskStack.Features.TaskStack.ViewModels;

namespace TaskStack.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddDbContext<TaskContext>();
        return services;
    }
}