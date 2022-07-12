using ManagedBass;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VolumeCorrector.Core;

namespace VolumeCorrector.Bass
{
    public class BassVolumeService : IVolumeService, IDisposable
    {
        private const int RecordChannelsCount = 2;

        private readonly ManagedBassConfiguration _configuration;
        private readonly ILogger<BassVolumeService> _logger;
        private int? _recordChannelId;
        private float _loudness;
        private bool _recordsReceived;

        public BassVolumeService(ILogger<BassVolumeService> logger, IOptions<ManagedBassConfiguration> options)
        {
            _configuration = options.Value;
            _logger = logger;
            _loudness = 0f;
            Initialized = false;
            _recordsReceived = false;
        }

        public static IEnumerable<DeviceData> GetDevices()
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

        public static IEnumerable<DeviceData> GetRecordDevices()
        {
            bool hasNext = true;
            var deviceId = 1;

            while (hasNext)
            {
                hasNext = ManagedBass.Bass.RecordGetDeviceInfo(deviceId, out var device);

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

        public bool Initialized { get; private set; }

        public float GetVolume()
        {
            return (float)ManagedBass.Bass.Volume;
        }

        public void SetVolume(float volume)
        {
            try
            {
                ManagedBass.Bass.Volume = volume;
            }
            catch (BassException ex)
            {
                _logger.LogError(ex, "Bass error on setting volume: {ErrorCode}", Enum.GetName(ex.ErrorCode));
                throw;
            }
        }

        public float GetLoudness()
        {
            return _loudness;
        }

        public void Dispose()
        {
            ManagedBass.Bass.Free();
        }

        public Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Volume service initialization.");

                var volumeDeviceId = GetVolumeDeviceId();
                var recordDeviceId = GetRecordDeviceId();

                ManagedBass.Bass.Init(volumeDeviceId);

                ManagedBass.Bass.RecordInit(recordDeviceId);
                _recordChannelId = ManagedBass.Bass.RecordStart(44100, RecordChannelsCount, BassFlags.Byte, RecordHandler);

                Initialized = true;

                return Task.CompletedTask;
            }
            catch (BassException ex)
            {
                _logger.LogError(ex, "Bass error on initialization: {ErrorCode}", Enum.GetName(ex.ErrorCode));
                throw;
            }
        }

        public Task UpdateAsync()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException(
                    $"{nameof(BassVolumeService)}.{nameof(InitializeAsync)} should be called before usage.");
            }

            if (!_recordsReceived)
            {
                _logger.LogWarning("No audio info received so far.");
            }

            try
            {
                var loudness = GetSoundLoudness();

                if (loudness[0] > float.Epsilon || loudness[1] > float.Epsilon)
                {
                    _loudness = loudness.Average();
                }
            }
            catch (BassException ex)
            {
                _logger.LogError(ex, "Bass error on update: {ErrorCode}", Enum.GetName(ex.ErrorCode));
                throw;
            }

            return Task.CompletedTask;
        }

        private float[] GetSoundLoudness()
        {
            var levels = new float[2];
            var success = ManagedBass.Bass.ChannelGetLevel(_recordChannelId!.Value, levels, 0.01f, LevelRetrievalFlags.All);

            if (!success)
            {
                throw new BassException(ManagedBass.Bass.LastError);
            }

            return levels;
        }

        private int GetVolumeDeviceId()
        {
            var devices = GetDevices().ToArray();

            if (string.IsNullOrEmpty(_configuration.VolumeDeviceName))
            {
                _logger.LogInformation("Volume device: Default");
                return -1;
            }

            var volumeDevice = devices.FirstOrDefault(dev => dev.Info.Name == _configuration.VolumeDeviceName);

            if (volumeDevice == null)
            {
                _logger.LogInformation(
                    "Unable to find volume device \"{DeviceName}\". Found devices: {Devices}",
                    _configuration.VolumeDeviceName,
                    devices.Select(dev => $"{dev.Info.Name} (Default: {dev.Info.IsDefault})"));
                throw new Exception("Unable to find volume device.");
            }

            _logger.LogInformation("Volume device: {DeviceName}", volumeDevice.Info.Name);

            return volumeDevice.Id;
        }

        private int GetRecordDeviceId()
        {
            var devices = GetRecordDevices().ToArray();

            if (string.IsNullOrEmpty(_configuration.RecordDeviceName))
            {
                _logger.LogInformation("Record device: Default");
                return -1;
            }

            var recordDevice = devices.FirstOrDefault(dev => dev.Info.Name == _configuration.RecordDeviceName);

            if (recordDevice == null)
            {
                _logger.LogInformation(
                    "Unable to find record device \"{DeviceName}\". Found devices: {Devices}",
                    _configuration.RecordDeviceName,
                    devices.Select(dev => $"{dev.Info.Name} (Default: {dev.Info.IsDefault})"));
                throw new Exception("Unable to find record device.");
            }

            _logger.LogInformation("Record device: {DeviceName}", recordDevice.Info.Name);

            return recordDevice.Id;
        }

        private bool RecordHandler(int handler, IntPtr buffer, int length, IntPtr user)
        {
            if (buffer != IntPtr.Zero && length > 0)
            {
                if (!_recordsReceived)
                {
                    _logger.LogInformation("First audio record received.");
                }

                _recordsReceived = true;
            }

            return true;
        }
    }
}