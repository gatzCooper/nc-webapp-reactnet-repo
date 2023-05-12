
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using nc_attendance_app_api.BusinessLayer;
using nc_attendance_app_api.DAL;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Services;
using nc_attendance_app_api.Utilities;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var services = builder.Services;


services.AddLogging();

builder.Services
    .AddScoped<IDataAccessService, DataAccessService>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IUserBusinessLayer, UserBusinessLayer>()
    .AddScoped<IScheduleService, ScheduleService>()
    .AddScoped<IScheduleBusinessLayer, ScheduleBusinessLayer>()
    .AddScoped<ISubjectBusinessLayer, SubjectBusinessLayer>()
    .AddScoped<ISubjectService, SubjectService>()
    .AddScoped<IAttendanceBusinessLayer, AttendanceBusinessLayer>()
    .AddScoped<IAttendanceService, AttendanceService>()
    .AddScoped<IDashboardBusinessLayer, DashboardBusinessLayer>()
    .AddScoped<IDashboardService, DashboardService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connection strings
var ConnectionStrings = new ConnectionStrings();

builder.Configuration.Bind(nameof(ConnectionStrings), ConnectionStrings);
builder.Services.AddSingleton(ConnectionStrings);

//allow cors
var corsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    });
});



// Configuration
var configurationBuilder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json");

var configuration = configurationBuilder.Build();

var app = builder.Build();



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

