using System.Reflection;
using System.Text;
using ToDoList.Services.Extensions;
using ToDoList.Services.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Services.Infrastructures.jwt;
using ToDoList.Services.Handlers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApp", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            },
    });
});
builder.Services.AddDatabaseConnection();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(jwt =>
        {

            JwtConfig jwtConfig = builder.Configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                ClockSkew = TimeSpan.Zero
            };
        });

builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("Authorization", policy => policy.Requirements.Add(new AuthorizationRequirment()));
});
builder.Services.BindConfigurations(builder.Configuration);
//Add automapper
builder.Services.AddAutoMapper(Assembly.Load("ToDoList.Models"));
builder.Services.RegisterServices();
builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
var app = builder.Build();
//var context = app.Services.CreateScope().ServiceProvider.GetService<ApplicationDBContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("AllowAll");
app.ConfigureException(builder.Environment);
//await context.RunPendingMigrationsAsync();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.UseStaticFiles();



app.Run();
