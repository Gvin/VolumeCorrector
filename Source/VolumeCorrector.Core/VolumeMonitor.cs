using System;
using Microsoft.Extensions.Options;
using VolumeCorrector.Core.Configuration;

namespace VolumeCorrector.Core
{
    public class VolumeMonitor : IVolumeMonitor
    {
        private readonly IVolumeService _volumeService;
        private readonly ICorrectionStrategy _correctionStrategy;
        private readonly VolumeCorrectorConfiguration _configuration;
        private bool _enabled;
        private float? _storedSystemVolume;

        public VolumeMonitor(
            IVolumeService volumeService,
            ICorrectionStrategy correctionStrategy,
            IOptions<VolumeCorrectorConfiguration> options)
        {
            _volumeService = volumeService;
            _correctionStrategy = correctionStrategy;
            _configuration = options.Value;
            _storedSystemVolume = null;
        }

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
                        _storedSystemVolume = _volumeService.GetVolume();
                    }
                    else if (_storedSystemVolume.HasValue)
                    {
                        _volumeService.SetVolume(_storedSystemVolume.Value);
                    }
                }
            }
        }

        public void Update()
        {
            var volume = _volumeService.GetVolume();
            var loudness = _volumeService.GetLoudness();

            var realMaxVolume = _configuration.MaxVolume / 100f;
            var realMaxLoudness = _configuration.MaxLoudness / 100f;

            var targetVolume = _correctionStrategy.GetTargetVolume(volume, loudness, realMaxVolume, realMaxLoudness);

            if (Math.Abs(targetVolume - volume) > float.Epsilon)
            {
                _volumeService.SetVolume(targetVolume);
            }
        }
    }
}