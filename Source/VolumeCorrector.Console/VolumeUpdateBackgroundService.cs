using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VolumeCorrector.Core;

namespace VolumeCorrector.Console;

public class VolumeUpdateBackgroundService : BackgroundService
{
    private const int UpdateIntervalMilliseconds = 100;

    private readonly IVolumeService _volumeService;
    private readonly IVolumeMonitor _volumeMonitor;
    private readonly ILogger<VolumeUpdateBackgroundService> _logger;

    public VolumeUpdateBackgroundService(
        ILogger<VolumeUpdateBackgroundService> logger,
        IVolumeService volumeService,
        IVolumeMonitor volumeMonitor)
    {
        _logger = logger;
        _volumeService = volumeService;
        _volumeMonitor = volumeMonitor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_volumeService.Initialized)
                {
                    await _volumeService.UpdateAsync();
                    _volumeMonitor.Update();
                }
                else
                {
                    _logger.LogWarning("Tried to update volume but service is not initialized.");
                }
                
                await Task.Delay(UpdateIntervalMilliseconds, stoppingToken);
            }
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
            _logger.LogError(ex, "Error while updating volume value.");
        }
    }
}