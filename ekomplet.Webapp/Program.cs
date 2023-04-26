using ekomplet.services;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IInstallerRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("LocalSQL");
    return new InstallerService(connectionString);
});

builder.Services.AddScoped<ISupervisorRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("LocalSQL");
    return new SupervisorService(connectionString);
});

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
