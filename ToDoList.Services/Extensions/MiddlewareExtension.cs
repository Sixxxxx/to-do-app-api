using ToDoList.Data.Context;
using ToDoList.Data.Interfaces;
using ToDoList.Services.Implementation;
using ToDoList.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Data.Implementation;
using ToDoList.Services.Handlers;
using ToDoList.Services.Implementation;
using ToDoList.Services.Infrastructure.jwt;

using Microsoft.AspNetCore.Identity;
using ToDoList.Services.Implementation;

namespace ToDoList.Services.Extensions
{
    public static class MiddlewareExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IJWTAuthenticator, JwtAuthenticator>();
            services.AddTransient<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddTransient<IUnitOfWork, UnitOfWork<ApplicationDBContext>>();
            services.AddTransient<IServiceFactory, ServiceFactory>();
            services.AddTransient<ITriangleService, TriangleService>();
            //services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<Interfaces.IAuthenticationService, Implementation.AuthenticationService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IStaffService, StaffService>();
            services.AddTransient<IToDoItemService, ToDoItemService>();
        }
    }
}
