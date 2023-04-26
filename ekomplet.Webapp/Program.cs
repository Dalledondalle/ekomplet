using ekomplet.Application.Services;
using ekomplet.Domain;
using ekomplet.Domain.Interfaces;
using ekomplet.Infrastructure.Repositories;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<IInstallerRepository, InstallerRepository>();
builder.Services.AddScoped<ISupervisorRepository, SupervisorRepository>();

builder.Services.AddScoped<IInstallerService, InstallerService>();
builder.Services.AddScoped<ISupervisorService, SupervisorService>();


ILogger logger = new LoggerConfiguration()
    .Enrich.WithExceptionDetails()
    .WriteTo.RollingFile(
        new JsonFormatter(renderMessage: true),
        @"logs\log-{Date}.txt")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
