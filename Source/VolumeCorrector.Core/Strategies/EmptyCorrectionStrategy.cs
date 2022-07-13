namespace VolumeCorrector.Core.Strategies
{
    public class EmptyCorrectionStrategy : ICorrectionStrategy
    {
        public double GetTargetVolume(double volume, float loudness, double maxVolume, float maxLoudness)
        {
            return volume;
        }
    }
}
