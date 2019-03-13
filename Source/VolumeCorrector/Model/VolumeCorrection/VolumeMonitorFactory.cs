using VolumeCorrector.VolumeCorrection.Strategies;

namespace VolumeCorrector.Model.VolumeCorrection
{
    public class VolumeMonitorFactory : IVolumeMonitorFactory
    {
        public IVolumeMonitor Create()
        {
            return new VolumeMonitor(new MediumCorrectionStrategy());
        }
    }
}