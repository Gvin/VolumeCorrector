namespace VolumeCorrector.Core
{
    public interface IVolumeService
    {
        float GetVolume();

        void SetVolume(float volume);

        float GetLoudness();

        void Initialize();

        void Update();
    }
}