using Backend.Modules.TaskItem.Commands;
using Backend.Modules.TaskItem.Queries;

namespace Backend.Modules.TaskItem
{
    public static class TaskItemDependencyInjection
    {
        public static IServiceCollection AddTaskItemModule(this IServiceCollection services)
        {
            services.AddScoped<AddTodoCommandHandler>();
            services.AddScoped<UpdateTodoCommandHandler>();
            services.AddScoped<DeleteTodoCommandHandler>();
            services.AddScoped<ToggleStatusCommandHandler>();
            services.AddScoped<GetTodosQueryHandler>();
            services.AddScoped<GetTodoByIdQueryHandler>();
            return services;
        }
    }
}