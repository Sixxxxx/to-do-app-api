using ToDoList.Data.Context;
using ToDoList.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class SqlClientRegistrationExtension
{
    public static void AddDatabaseConnection(this IServiceCollection services)
    {
        IConfiguration config;

        using (var serviceProvider = services.BuildServiceProvider())
        {
            config = serviceProvider.GetService<IConfiguration>();
        }
        string cc = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
       .AddDefaultTokenProviders()
       .AddEntityFrameworkStores<ApplicationDBContext>();

    }
}