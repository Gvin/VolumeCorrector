namespace VolumeCorrector.Core
{
    public interface IVolumeMonitor
    {
        bool Enabled { get; set; }

        void Update();
    }
}
