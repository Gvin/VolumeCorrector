using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using VolumeCorrector.VolumeCorrection;

namespace VolumeCorrector.Model.VolumeCorrection
{
    /// <summary>
    /// Monitors volume and loudness values.
    /// Can perform some actions on them if configured.
    /// </summary>
    public class VolumeMonitor : IDisposable, IVolumeMonitor
    {
        private const int CheckInterval = 10;

        private float loudness;
        private int maxLoudness;
        private float volume;
        private int maxVolume;
        private Task checkSoundTask;
        private bool enabled;

        public event EventHandler StatusChanged;

        private readonly ICorrectionStrategy correctionStrategy;

        public VolumeMonitor(ICorrectionStrategy correctionStrategy)
        {
            this.correctionStrategy = correctionStrategy;

            maxVolume = 100;
            maxLoudness = 100;
        }

        public int Loudness => (int)Math.Round(loudness * 100);

        public int Volume => (int)Math.Round(volume * 100);

        public int MaxVolume
        {
            get => maxVolume;
            set => maxVolume = value;
        }

        public int MaxLoudness
        {
            get => maxLoudness;
            set => maxLoudness = value;
        }

        public bool Enabled => enabled;

        public void Start()
        {
            if (checkSoundTask != null)
                throw new InvalidOperationException("Volume meter has already started.");

            enabled = true;
            checkSoundTask = Task.Factory.StartNew(CheckSound);

            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            if (checkSoundTask == null)
                throw new InvalidOperationException("Volume meter has not started.");

            enabled = false;
            checkSoundTask.Wait();
            checkSoundTask.Dispose();
            checkSoundTask = null;

            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CheckSound()
        {
            while (enabled)
            {
                try
                {
                    CheckAndCorrectSound();
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                }
                Thread.Sleep(CheckInterval);
            }
        }

        private void CheckAndCorrectSound()
        {
            var realMaxVolume = maxVolume / 100f;
            var realMaxLoudness = maxLoudness / 100f;

            using (var device = GetCurrentAudioDevice())
            {
                loudness = GetCurrentLoudness(device);
                volume = GetCurrentMasterVolume(device);

                var targetVolume = correctionStrategy.GetTargetVolume(volume, loudness, realMaxVolume, realMaxLoudness);
                SetVolume(device, targetVolume);
            }
        }

        private float GetCurrentMasterVolume(MMDevice device)
        {
            return device.AudioEndpointVolume.MasterVolumeLevelScalar;
        }

        private MMDevice GetCurrentAudioDevice()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            }
        }

        private float GetCurrentLoudness(MMDevice device)
        {
            return device.AudioMeterInformation.MasterPeakValue;
        }

        private void SetVolume(MMDevice device, float value)
        {
            device.AudioEndpointVolume.MasterVolumeLevelScalar = value;
        }

        public void Dispose()
        {
            enabled = false;
            checkSoundTask?.Wait();
            checkSoundTask?.Dispose();
        }
    }
}