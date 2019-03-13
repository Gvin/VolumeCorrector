using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using VolumeCorrector.Model.VolumeCorrection;
using VolumeCorrector.Properties;
using VolumeCorrector.Views;
using Timer = System.Windows.Forms.Timer;

namespace VolumeCorrector.Presenters
{
    public class OptionsPresenter : IOptionsPresenter
    {
        private readonly IOptionsView view;
        private readonly IVolumeMonitor volumeMonitor;
        private readonly Timer volumeStatusTimer;

        public event EventHandler Closed;

        public OptionsPresenter(IOptionsView view, IVolumeMonitor volumeMonitor)
        {
            this.view = view;
            this.volumeMonitor = volumeMonitor;
            volumeStatusTimer = new Timer
            {
                Interval = 100,
                Enabled = true
            };
            volumeStatusTimer.Tick += volumeStatusTimer_Tick;

            view.MaxVolume = volumeMonitor.MaxVolume;
            view.MaxLoudness = volumeMonitor.MaxLoudness;
            view.CultureCode = GetCurrentCultureCode();
            RefreshSoundMetrics();

            this.view.MaxVolumeChanged += view_MaxVolumeChanged;
            this.view.MaxLoudnessChanged += view_MaxLoudnessChanged;
            this.view.CultureCodeChanged += view_CultureCodeChanged;
            this.view.Closed += view_Closed;
        }

        private string GetCurrentCultureCode()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public void Run()
        {
            view.Show();
        }

        public void BringToFront()
        {
            view.ForceBringToFront();
        }

        private void volumeStatusTimer_Tick(object sender, EventArgs args)
        {
            RefreshSoundMetrics();
        }

        private void RefreshSoundMetrics()
        {
            view.Volume = volumeMonitor.Volume;
            view.Loudness = volumeMonitor.Loudness;
        }

        private void view_Closed(object sender, EventArgs args)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void view_MaxVolumeChanged(object sender, EventArgs args)
        {
            volumeMonitor.MaxVolume = view.MaxVolume;
            Settings.Default.MaxVolume = view.MaxVolume;
            Settings.Default.Save();
        }

        private void view_MaxLoudnessChanged(object sender, EventArgs args)
        {
            SetMaxLoudness(view.MaxLoudness);
        }

        private void SetMaxLoudness(int value)
        {
            volumeMonitor.MaxLoudness = value;
            Settings.Default.MaxLoudness = value;
            Settings.Default.Save();
        }

        private void view_CultureCodeChanged(object sender, EventArgs args)
        {
            Settings.Default.LanguageCode = view.CultureCode;
            Settings.Default.Save();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(view.CultureCode);

            MessageBox.Show(Resources.LanguageChange_Text, Resources.LanguageChange_Caption, MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public void Dispose()
        {
            view.MaxVolumeChanged -= view_MaxVolumeChanged;
            view.MaxLoudnessChanged -= view_MaxLoudnessChanged;
            view.CultureCodeChanged -= view_CultureCodeChanged;
            view.Closed -= view_Closed;
            volumeStatusTimer.Tick -= volumeStatusTimer_Tick;
        }
    }
}