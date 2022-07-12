using Microsoft.Extensions.Logging;
using VolumeCorrector.Core;

namespace VolumeCorrector.Console;

public class ConsoleUserInterface
{
    private const int InfoRefreshDelayMilliseconds = 300;

    private readonly IVolumeService _volumeService;
    private readonly IVolumeMonitor _volumeMonitor;
    private readonly ILogger<ConsoleUserInterface> _logger;

    public ConsoleUserInterface(IVolumeService volumeService, IVolumeMonitor volumeMonitor, ILogger<ConsoleUserInterface> logger)
    {
        _volumeService = volumeService;
        _volumeMonitor = volumeMonitor;
        _logger = logger;
    }

    public void Start()
    {
        try
        {
            StartImpl();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in console UI.");
        }
    }

    private void StartImpl()
    {
        var processCancellationTokenSource = new CancellationTokenSource();

        System.Console.Clear();

        System.Console.CursorTop = 5;
        System.Console.CursorLeft = 0;
        System.Console.CursorVisible = false;

        System.Console.WriteLine(" Commands:");
        System.Console.WriteLine(" Esc - Exit");
        System.Console.WriteLine(" e - Enable");
        System.Console.WriteLine(" d - Disable");
        System.Console.WriteLine(" ^ - Inc. max Volume");
        System.Console.WriteLine(" v - Dec. max Volume");
        System.Console.WriteLine(" > - Inc. max Loudness");
        System.Console.WriteLine(" < - Dec. max Loudness");

#pragma warning disable CS4014
        UpdateConsoleInfo(processCancellationTokenSource.Token);
#pragma warning restore CS4014

        while (true)
        {
            var key = System.Console.ReadKey();

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }

            if (key.Key == ConsoleKey.UpArrow)
            {
                _volumeMonitor.MaxVolume = Math.Min(100, _volumeMonitor.MaxVolume + 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                _volumeMonitor.MaxVolume = Math.Max(0, _volumeMonitor.MaxVolume - 1);
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                _volumeMonitor.MaxLoudness = Math.Max(0, _volumeMonitor.MaxLoudness - 1);
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                _volumeMonitor.MaxLoudness = Math.Min(100, _volumeMonitor.MaxLoudness + 1);
            }
            else if (key.Key == ConsoleKey.E)
            {
                _volumeMonitor.Enabled = true;
            }
            else if (key.Key == ConsoleKey.D)
            {
                _volumeMonitor.Enabled = false;
            }
        }

        _volumeMonitor.Enabled = false;

        processCancellationTokenSource.Cancel();
    }

    private async Task UpdateConsoleInfo(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var volume = (int)Math.Floor(_volumeService.GetVolume() * 100);
                var loudness = (int)Math.Floor(_volumeService.GetLoudness() * 100);

                System.Console.SetCursorPosition(0, 1);

                var statusText = _volumeMonitor.Enabled ? "Enabled" : "Disabled";
                System.Console.WriteLine($" Status: {statusText}     ");
                System.Console.WriteLine($" Max Volume: {_volumeMonitor.MaxVolume} Max Loudness: {_volumeMonitor.MaxLoudness}   ");
                System.Console.WriteLine($" Volume: {volume} Loudness: {loudness}               ");

                System.Console.SetCursorPosition(0, 13);
                System.Console.WriteLine("     ");
                System.Console.SetCursorPosition(0, 13);

                await Task.Delay(TimeSpan.FromMilliseconds(InfoRefreshDelayMilliseconds), token);
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
                _logger.LogError(ex, "Error while updating console info.");
            }
        }
    }
}
