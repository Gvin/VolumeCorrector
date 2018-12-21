namespace VolumeCorrector.VolumeCorrection.Strategies
{
    public class MeasuringCorrectionStrategy : ICorrectionStrategy
    {
        private float highestLoudness;

        public MeasuringCorrectionStrategy()
        {
            highestLoudness = float.MinValue;
        }

        public float HighestLoudness => highestLoudness;

        public float GetTargetVolume(float volume, float loudness, float maxVolume, float maxLoudness)
        {
            if (loudness > highestLoudness)
            {
                highestLoudness = loudness;
            }

            return volume;
        }
    }
}