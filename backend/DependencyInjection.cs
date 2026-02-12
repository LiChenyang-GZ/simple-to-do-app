using Backend.Commands;
using Backend.Queries;

namespace Backend
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCqrsHandlers(this IServiceCollection services)
        {
            services.AddScoped<AddTodoCommandHandler>();
            services.AddScoped<UpdateTodoCommandHandler>();
            services.AddScoped<DeleteTodoCommandHandler>();
            services.AddScoped<GetTodosQueryHandler>();
            services.AddScoped<GetTodoByIdQueryHandler>();
            return services;
        }
    }
}