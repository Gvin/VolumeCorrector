namespace VolumeCorrector.Model.VolumeCorrection.Strategies
{
    public class MediumCorrectionStrategy : ICorrectionStrategy
    {
        public float GetTargetVolume(float volume, float loudness, float maxVolume, float maxLoudness)
        {
            var distance = maxLoudness - loudness;
            if (distance < 0)
                distance = 0;

            var targetAbsolute = (0.5f + distance);
            if (targetAbsolute > 1f)
                targetAbsolute = 1f;

            return targetAbsolute * maxVolume;
        }
    }
}