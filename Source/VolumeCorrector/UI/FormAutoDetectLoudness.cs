using System;
using System.Windows.Forms;
using VolumeCorrector.VolumeCorrection;
using VolumeCorrector.VolumeCorrection.Strategies;

namespace VolumeCorrector.UI
{
    public partial class FormAutoDetectLoudness : Form
    {
        private readonly VolumeMonitor monitor;
        private readonly MeasuringCorrectionStrategy strategy;

        public FormAutoDetectLoudness()
        {
            InitializeComponent();

            strategy = new MeasuringCorrectionStrategy();
            monitor = new VolumeMonitor(strategy);
        }

        public int MaxLoudness =>(int) Math.Round(strategy.HighestLoudness * 100);

        private void FormAutoDetectLoudness_Shown(object sender, EventArgs e)
        {
            monitor.Start();
        }

        private void buttonReady_Click(object sender, EventArgs e)
        {
            monitor.Stop();
            Close();
        }

        private void timerUpdateLoudnessValue_Tick(object sender, EventArgs e)
        {
            progressBarLoudness.Value = monitor.Loudness;
            labelLoudness.Text = monitor.Loudness.ToString();
        }

        public new void Dispose()
        {
            base.Dispose();

            monitor.Dispose();
        }
    }
}
