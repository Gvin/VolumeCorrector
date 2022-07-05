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

var logger = application.Services.GetRequiredService<ILogger<Program>>();

var processCancellationTokenSource = new CancellationTokenSource();

logger.LogInformation("Starting application.");

await application.StartAsync();

Console.Clear();

Console.CursorTop = 4;
Console.CursorLeft = 1;
Console.CursorVisible = false;

Console.WriteLine("Press ENTER to close the application.");

#pragma warning disable CS4014
Task.Run(async () =>
{
    await UpdateConsoleInfo(processCancellationTokenSource.Token);
});
#pragma warning restore CS4014

Console.ReadLine();

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

            Console.CursorTop = 1;
            Console.CursorLeft = 1;
            Console.WriteLine($"Volume: {volume} Loudness: {loudness}               ");

            await Task.Delay(TimeSpan.FromSeconds(1), token);
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