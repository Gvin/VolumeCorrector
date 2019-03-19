using NUnit.Framework;
using VolumeCorrector.Model.VolumeCorrection;
using VolumeCorrector.Model.VolumeCorrection.Strategies;

namespace VolumeCorrector.Test.Model
{
    [TestFixture]
    public class EmptyCorrectionStrategyTest
    {
        [Test]
        public void ShouldNotChangeAnything()
        {
            ICorrectionStrategy strategy = new EmptyCorrectionStrategy();
            const float volume = 0.5f;
            const float loudness = 0.7f;
            const float maxVolume = 0.3f;
            const float maxLoudness = 0.1f;

            var targetVolume = strategy.GetTargetVolume(volume, loudness, maxVolume, maxLoudness);
            Assert.AreEqual(volume, targetVolume);
        }
    }
}
