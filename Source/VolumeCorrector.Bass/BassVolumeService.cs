using ManagedBass;
using Microsoft.Extensions.Logging;
using VolumeCorrector.Core;

namespace VolumeCorrector.Bass
{
    public class BassVolumeService : IVolumeService, IDisposable
    {
        private const int RecordChannelsCount = 2;

        private readonly ILogger<BassVolumeService> _logger;
        private int? _recordChannelId;
        private float _loudness;

        public BassVolumeService(ILogger<BassVolumeService> logger)
        {
            _logger = logger;
            _loudness = 0f;
        }

        public float GetVolume()
        {
            return (float)ManagedBass.Bass.Volume;
        }

        public void SetVolume(float volume)
        {
            ManagedBass.Bass.Volume = volume;
        }

        public float GetLoudness()
        {
            return _loudness;
        }

        public void Dispose()
        {
            ManagedBass.Bass.Free();
        }

        public void Initialize()
        {
            _logger.LogInformation("Volume service initialization.");

            var devices = GetDevices().ToArray();
            var defaultDevice = devices.First(dev => dev.Info.IsDefault && dev.Info.Name != "Default");

            ManagedBass.Bass.Init(defaultDevice.Id);

            ManagedBass.Bass.GetInfo(out var deviceInfo);

            ManagedBass.Bass.RecordInit(defaultDevice.Id);
            _recordChannelId = ManagedBass.Bass.RecordStart(deviceInfo.MinSampleRate, RecordChannelsCount, BassFlags.Byte, null);
        }

        public void Update()
        {
            if (!_recordChannelId.HasValue)
            {
                throw new InvalidOperationException(
                    $"{nameof(BassVolumeService)}.{nameof(Initialize)} should be called before usage.");
            }

            var loudness = GetSoundLoudness();
            if (loudness[0] < float.Epsilon && loudness[1] < float.Epsilon)
            {
                return;
            }

            _loudness = loudness.Average();
        }

        private float[] GetSoundLoudness()
        {
            var levels = new float[2];
            ManagedBass.Bass.ChannelGetLevel(_recordChannelId!.Value, levels, 0.01f, LevelRetrievalFlags.All);

            return levels;
        }

        private static IEnumerable<DeviceData> GetDevices()
        {
            bool hasNext = true;
            var deviceId = 1;

            while (hasNext)
            {
                hasNext = ManagedBass.Bass.GetDeviceInfo(deviceId, out var device);
                if (hasNext)
                {
                    yield return new DeviceData
                    {
                        Id = deviceId,
                        Info = device
                    };
                }

                deviceId++;
            }
        }

        private class DeviceData
        {
            public DeviceInfo Info { get; init; }

            public int Id { get; init; }
        }
    }
}