using ToDoList.Services.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Services.Infrastructures.jwt;
using ToDoList.Services.Infrastructure;
using ToDoList.Services.Infrastructures.jwt;

namespace ToDoList.Services.Extensions
{
    public static class ConfigurationBinder
    {
        public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            JwtConfig jwt = new();
            AppConstants appConstants = new();
    

            configuration.GetSection("JwtConfig").Bind(jwt);
            configuration.GetSection("AppConstants").Bind(appConstants);
            
            services.AddSingleton(jwt);
            services.AddSingleton(appConstants);
            

            return services;
        }
    }
}
