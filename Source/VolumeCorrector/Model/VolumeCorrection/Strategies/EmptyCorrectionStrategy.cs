namespace VolumeCorrector.Model.VolumeCorrection.Strategies
{
    public class EmptyCorrectionStrategy : ICorrectionStrategy
    {
        public float GetTargetVolume(float volume, float loudness, float maxVolume, float maxLoudness)
        {
            return volume;
        }
    }
}