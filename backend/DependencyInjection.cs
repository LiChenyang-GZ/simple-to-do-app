using Backend.Modules.TaskItem;

namespace Backend
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddModule(this IServiceCollection services)
        {
            services.AddTaskItemModule();
            return services;
        }
    }
}