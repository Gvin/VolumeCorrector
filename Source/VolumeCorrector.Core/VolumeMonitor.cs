using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VolumeCorrector.Core.Configuration;

namespace VolumeCorrector.Core
{
    public class VolumeMonitor : IVolumeMonitor
    {
        private readonly IVolumeService _volumeService;
        private readonly ICorrectionStrategy _correctionStrategy;
        private readonly ILogger<VolumeMonitor> _logger;
        private bool _enabled;
        private double? _storedSystemVolume;

        public VolumeMonitor(
            IVolumeService volumeService,
            ICorrectionStrategy correctionStrategy,
            IOptions<VolumeCorrectorConfiguration> options,
            ILogger<VolumeMonitor> logger)
        {
            _volumeService = volumeService;
            _correctionStrategy = correctionStrategy;
            _logger = logger;
            _storedSystemVolume = null;

            MaxVolume = options.Value.MaxVolume;
            MaxLoudness = options.Value.MaxLoudness;

            Enabled = true;
        }

        public int MaxVolume { get; set; }

        public int MaxLoudness { get; set; }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                var oldValue = _enabled;
                _enabled = value;

                if (oldValue != value)
                {
                    if (_enabled)
                    {
                        _logger.LogInformation("Enabling volume correction.");
                        _storedSystemVolume = _volumeService.GetVolume();
                    }
                    else
                    {
                        _logger.LogInformation("Disabling volume correction.");
                        if (_storedSystemVolume.HasValue)
                        {
                            _volumeService.SetVolume(_storedSystemVolume.Value);
                        }
                    }
                }
            }
        }

        public void Update()
        {
            if (Enabled)
            {
                var volume = _volumeService.GetVolume();
                var loudness = _volumeService.GetLoudness();

                var realMaxVolume = MaxVolume / 100D;
                var realMaxLoudness = MaxLoudness / 100f;

                var targetVolume = _correctionStrategy.GetTargetVolume(volume, loudness, realMaxVolume, realMaxLoudness);

                if (Math.Abs(targetVolume - volume) > float.Epsilon)
                {
                    _volumeService.SetVolume(targetVolume);
                }
            }
        }
    }
}