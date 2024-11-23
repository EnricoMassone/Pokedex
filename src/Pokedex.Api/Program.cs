using Pokedex.Application;
using Pokedex.Infrastructure;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Globalization;
using System.Reflection;

namespace Pokedex.Api;

public static class Program
{
  public static int Main(string[] args)
  {
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console(
          outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
          formatProvider: CultureInfo.InvariantCulture,
          theme: AnsiConsoleTheme.Code
        )
        .CreateBootstrapLogger();

    Log.Information("Starting up!");

    try
    {
      var builder = WebApplication.CreateBuilder(args);

      // Setup Serilog logging
      builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
         loggerConfiguration
          .ReadFrom.Configuration(builder.Configuration)
          .ReadFrom.Services(serviceProvider)
      );

      // Add services to the container.
      builder.Services.AddControllers();

      builder.Services.AddApplication();
      builder.Services.AddInfrastructure(builder.Configuration);

      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();

      builder.Services.AddSwaggerGen(options =>
      {
        // using System.Reflection;
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
      });

      var app = builder.Build();

      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/error");
      }

      app.UseSerilogRequestLogging();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseAuthorization();

      app.MapControllers();

      app.Run();

      Log.Information("Stopped cleanly");

      return 0;
    }
    catch (Exception exception)
    {
      Log.Fatal(exception, "An unhandled exception occurred during bootstrapping");
      return 1;
    }
    finally
    {
      Log.CloseAndFlush();
    }
  }
}
