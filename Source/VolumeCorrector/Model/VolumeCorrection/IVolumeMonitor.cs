namespace VolumeCorrector.Model.VolumeCorrection
{
    public interface IVolumeMonitor
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