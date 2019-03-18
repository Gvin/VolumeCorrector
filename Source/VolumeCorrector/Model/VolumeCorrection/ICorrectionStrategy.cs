using Gvin.Injection;

namespace VolumeCorrector.Model.VolumeCorrection
{
    public interface ICorrectionStrategy
    {
        float GetTargetVolume(float volume, float loudness, float maxVolume, float maxLoudness);
    }
}