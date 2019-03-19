using NUnit.Framework;
using VolumeCorrector.Model.VolumeCorrection.Strategies;

namespace VolumeCorrector.Test.Model
{
    [TestFixture]
    public class MediumCorrectionStrategyTest
    {
        [Test]
        public void ShouldNeverSetVolumeGreaterThanMax()
        {
            const float maxVolume = 0.3f;
            const float volume = 0.7f;
            const float loudness = 0f;
            const float maxLoudness = 1f;

            var strategy = new MediumCorrectionStrategy();
            var targetVolume = strategy.GetTargetVolume(volume, loudness, maxVolume, maxLoudness);
            Assert.AreEqual(maxVolume, targetVolume);
        }

        [Test]
        public void ShouldReactToLoudnessGreaterThanMax()
        {
            const float maxVolume = 0.3f;
            const float volume = 0.7f;
            const float loudness = 0.5f;
            const float maxLoudness = 0.2f;

            var strategy = new MediumCorrectionStrategy();
            var targetVolume = strategy.GetTargetVolume(volume, loudness, maxVolume, maxLoudness);
            Assert.Less(targetVolume, volume);
        }
    }
}