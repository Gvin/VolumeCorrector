using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using VolumeCorrector.Bass;
using VolumeCorrector.Console;
using VolumeCorrector.Core;
using VolumeCorrector.Core.Configuration;
using VolumeCorrector.Core.Strategies;

const string logMessageTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
const string logFileName = "log.log";

if (args.Length == 1)
{
    if (args[0].ToLower() == "devices")
    {
        Console.WriteLine("Getting the list of all devices...");

        foreach (var deviceData in BassVolumeService.GetDevices())
        {
            Console.WriteLine($"Name: \"{deviceData.Info.Name}\"; IsDefault: {deviceData.Info.IsDefault}; Type: {Enum.GetName(deviceData.Info.Type)}");
        }
    }
    else if (args[0].ToLower() == "recorddevices")
    {
        Console.WriteLine("Getting the list of record devices...");

        foreach (var deviceData in BassVolumeService.GetRecordDevices())
        {
            Console.WriteLine($"Name: \"{deviceData.Info.Name}\"; IsDefault: {deviceData.Info.IsDefault}; Type: {Enum.GetName(deviceData.Info.Type)}");
        }
    }

    return;
}

var builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((_, configBuilder) =>
{
    configBuilder.Add(new JsonConfigurationSource
    {
        Path = "./appsettings.json"
    });

#if DEBUG_WIN || RELEASE_WIN
    configBuilder.Add(new JsonConfigurationSource
    {
        Path = "./appsettings.WIN.json"
    });
#endif

#if DEBUG_LINUX || RELEASE_LINUX
    configBuilder.Add(new JsonConfigurationSource
    {
        Path = "./appsettings.LINUX.json"
    });
#endif
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(new MessageTemplateTextFormatter(logMessageTemplate), logFileName, LogEventLevel.Information)
    .CreateLogger();

builder.ConfigureServices((context, services) =>
{
    services.Configure<VolumeCorrectorConfiguration>(
        context.Configuration.GetSection("VolumeCorrector"));
    services.Configure<ManagedBassConfiguration>(
        context.Configuration.GetSection("ManagedBass"));

    services.AddLogging(logBuilder =>
    {
        logBuilder.AddSerilog();
    });

    services.AddSingleton<IVolumeService, BassVolumeService>();

    services
        .AddSingleton<IVolumeMonitor, VolumeMonitor>()
        .AddSingleton<ICorrectionStrategy, MediumCorrectionStrategy>();

    services.AddSingleton<ConsoleUserInterface>();

    services.AddHostedService<VolumeUpdateBackgroundService>();
});

var application = builder.Build();

var volumeService = application.Services.GetRequiredService<IVolumeService>();
var logger = application.Services.GetRequiredService<ILogger<Program>>();

try
{
    await volumeService.InitializeAsync();

    logger.LogInformation("Starting application.");

    await application.StartAsync();
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Error during initialization.");
    return;
}

var consoleInterface = application.Services.GetRequiredService<ConsoleUserInterface>();
consoleInterface.Start();

logger.LogInformation("Application exit.");
