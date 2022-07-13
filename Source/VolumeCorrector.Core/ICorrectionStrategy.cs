namespace VolumeCorrector.Core
{
    public interface ICorrectionStrategy
    {
        double GetTargetVolume(double volume, float loudness, double maxVolume, float maxLoudness);
    }
}
