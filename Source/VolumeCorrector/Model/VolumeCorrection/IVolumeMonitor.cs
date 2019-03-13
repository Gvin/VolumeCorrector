using System;
using Gvin.Injection;

namespace VolumeCorrector.Model.VolumeCorrection
{
    public interface IVolumeMonitor : IInjectable, IDisposable
    {
        int MaxVolume { get; set; }

        int MaxLoudness { get; set; }

        int Volume { get; }

        int Loudness { get; }

        bool Enabled { get; }

        void Start();

        void Stop();
    }
}