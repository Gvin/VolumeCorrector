namespace VolumeCorrector.Core
{
    public interface IVolumeMonitor
    {
        int MaxVolume { get; set; }

        int MaxLoudness { get; set; }

        bool Enabled { get; set; }

        void Update();
    }
}
