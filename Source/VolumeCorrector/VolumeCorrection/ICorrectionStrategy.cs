using NAudio.CoreAudioApi;

namespace VolumeCorrector.VolumeCorrection
{
    public interface ICorrectionStrategy
    {
        float GetTargetVolume(float volume, float loudness, float maxVolume, float maxLoudness);
    }
}