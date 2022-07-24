using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using pressF.API;
using pressF.API.Infra;
using Serilog;
using System;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);
    startup.ConfigureSerilogService(builder);

    var app = builder.Build();
    startup.Configure(app, app.Environment);
    startup.ConfigureSerilog(app);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}