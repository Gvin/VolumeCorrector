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

var builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(configBuilder =>
{
    configBuilder.Add(new JsonConfigurationSource
    {
        Path = "appsettings.json"
    });
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(new MessageTemplateTextFormatter(logMessageTemplate), logFileName, LogEventLevel.Information)
    .CreateLogger();

builder.ConfigureServices((context, services) =>
{
    services.Configure<VolumeCorrectorConfiguration>(
        context.Configuration.GetSection("VolumeCorrector"));

    services.AddLogging(logBuilder =>
    {
        logBuilder.AddSerilog();
    });

    services.AddSingleton<IVolumeService, BassVolumeService>();

    services
        .AddSingleton<IVolumeMonitor, VolumeMonitor>()
        .AddSingleton<ICorrectionStrategy, MediumCorrectionStrategy>();

    services.AddHostedService<VolumeUpdateBackgroundService>();
});

var application = builder.Build();

var volumeService = application.Services.GetRequiredService<IVolumeService>();
volumeService.Initialize();

var volumeMonitor = application.Services.GetRequiredService<IVolumeMonitor>();

var logger = application.Services.GetRequiredService<ILogger<Program>>();

var processCancellationTokenSource = new CancellationTokenSource();

logger.LogInformation("Starting application.");

await application.StartAsync();

Console.Clear();

Console.CursorTop = 5;
Console.CursorLeft = 0;
Console.CursorVisible = false;

Console.WriteLine(" Commands:");
Console.WriteLine(" Esc - Exit");
Console.WriteLine(" e - Enable");
Console.WriteLine(" d - Disable");
Console.WriteLine(" ^ - Inc. max Volume");
Console.WriteLine(" v - Dec. max Volume");
Console.WriteLine(" > - Inc. max Loudness");
Console.WriteLine(" < - Dec. max Loudness");

#pragma warning disable CS4014
Task.Run(async () =>
{
    await UpdateConsoleInfo(processCancellationTokenSource.Token);
});
#pragma warning restore CS4014

while (true)
{
    var key = Console.ReadKey();

    if (key.Key == ConsoleKey.Escape)
    {
        break;
    }

    if (key.Key == ConsoleKey.UpArrow)
    {
        volumeMonitor.MaxVolume = Math.Min(100, volumeMonitor.MaxVolume + 1);
    } 
    else if (key.Key == ConsoleKey.DownArrow)
    {
        volumeMonitor.MaxVolume = Math.Max(0, volumeMonitor.MaxVolume - 1);
    }
    else if (key.Key == ConsoleKey.LeftArrow)
    {
        volumeMonitor.MaxLoudness = Math.Max(0, volumeMonitor.MaxLoudness - 1);
    }
    else if (key.Key == ConsoleKey.RightArrow)
    {
        volumeMonitor.MaxLoudness = Math.Min(100, volumeMonitor.MaxLoudness + 1);
    }
    else if (key.Key == ConsoleKey.E)
    {
        volumeMonitor.Enabled = true;
    }
    else if (key.Key == ConsoleKey.D)
    {
        volumeMonitor.Enabled = false;
    }
}

volumeMonitor.Enabled = false;

processCancellationTokenSource.Cancel();

logger.LogInformation("Application exit.");

async Task UpdateConsoleInfo(CancellationToken token)
{
    while (!token.IsCancellationRequested)
    {
        try
        {
            var volume = (int)Math.Floor(volumeService.GetVolume() * 100);
            var loudness = (int)Math.Floor(volumeService.GetLoudness() * 100);

            Console.SetCursorPosition(0, 1);

            var statusText = volumeMonitor.Enabled ? "Enabled" : "Disabled";
            Console.WriteLine($" Status: {statusText}     ");
            Console.WriteLine($" Max Volume: {volumeMonitor.MaxVolume} Max Loudness: {volumeMonitor.MaxLoudness}   ");
            Console.WriteLine($" Volume: {volume} Loudness: {loudness}               ");

            Console.SetCursorPosition(0, 13);
            Console.WriteLine("     ");
            Console.SetCursorPosition(0, 13);

            await Task.Delay(TimeSpan.FromMilliseconds(300), token);
        }
        catch (TaskCanceledException)
        {
            // Do nothing
        }
        catch (OperationCanceledException)
        {
            // Do nothing
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating console info.");
        }
    }
}