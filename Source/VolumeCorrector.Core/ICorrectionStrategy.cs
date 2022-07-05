namespace VolumeCorrector.Core
{
    public interface ICorrectionStrategy
    {
        float GetTargetVolume(float volume, float loudness, float maxVolume, float maxLoudness);
    }
}
