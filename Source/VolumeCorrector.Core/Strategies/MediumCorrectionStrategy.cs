namespace VolumeCorrector.Core.Strategies
{
    public class MediumCorrectionStrategy : ICorrectionStrategy
    {
        public double GetTargetVolume(double volume, float loudness, double maxVolume, float maxLoudness)
        {
            var distance = maxLoudness - loudness;
            if (distance < 0)
                distance = 0;

            var targetAbsolute = (0.5D + distance);
            if (targetAbsolute > 1D)
                targetAbsolute = 1D;

            return targetAbsolute * maxVolume;
        }
    }
}
