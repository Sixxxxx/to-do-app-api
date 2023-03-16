using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SchMgr_FUPRE.Models.Enums;
using ToDoList.Services.Infrastructure;

namespace ToDoList.Services.Handlers
{
    public static class ExceptionHandler
    {
        public static void ConfigureException(this IApplicationBuilder app, IWebHostEnvironment hostEnvironment)
        {

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    IExceptionHandlerFeature? exceptionHandleFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandleFeature != null)
                    {
                        var status = 500;
                        switch (exceptionHandleFeature.Error)
                        {
                            //More Exceptions can be added as they are identified, those that aren't identified will default to the 500 status code 
                            case InvalidDataException:
                            case InvalidOperationException:
                            case KeyNotFoundException:
                            case ArgumentException:
                           
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                status = 400;
                                break;
                            case DbUpdateException:
                                context.Response.StatusCode = StatusCodes.Status409Conflict;
                                status = 409;
                                break;
                            default:
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                break;
                        }

                        ErrorResponse err = new() { Success = false, Status = status };
                        err.Message =
                            hostEnvironment.IsProduction() && context.Response.StatusCode ==
                            StatusCodes.Status500InternalServerError
                                ? "We currently cannot complete this request process. Please retry or contact our support"
                                : exceptionHandleFeature.Error.Message;

                        var serializerSettings = new JsonSerializerSettings();
                        serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        string msg =  JsonConvert.SerializeObject(err, serializerSettings);
                        await context.Response.WriteAsync(msg);
                    }
                });
            });
        }
    }
}
