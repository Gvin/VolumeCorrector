using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using VolumeCorrector.Model.VolumeCorrection;
using VolumeCorrector.Properties;
using VolumeCorrector.Views;
using VolumeCorrector.VolumeCorrection;
using Timer = System.Windows.Forms.Timer;

namespace VolumeCorrector.Presenters
{
    public class OptionsPresenter : IDisposable, IPresenter
    {
        private const int MinMaxLoudnessValue = 10;

        private readonly IOptionsView view;
        private readonly IVolumeMonitor volumeMonitor;
        private readonly Timer volumeStatusTimer;

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

            this.view.MaxVolumeChanged += view_MaxVolumeChanged;
            this.view.MaxLoudnessChanged += view_MaxLoudnessChanged;
            this.view.CultureCodeChanged += view_CultureCodeChanged;
            this.view.AutoDetectLoudnessClick += view_AutoDelectLoudnessClick;
            this.view.Closed += view_Closed;

            view.MaxVolume = volumeMonitor.MaxVolume;
            view.MaxLoudness = volumeMonitor.MaxLoudness;
            view.CultureCode = GetCurrentCultureCode();
            RefreshSoundMetrics();
        }

        private string GetCurrentCultureCode()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public void Run()
        {
            view.Show();
        }

        public DialogResult RunModal()
        {
            return view.ShowDialog();
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

        private void view_AutoDelectLoudnessClick(object sender, EventArgs args)
        {
            using (var presenter = new AutoDetectLoudnessPresenter(new FormAutoDetectLoudness()))
            {
                presenter.RunModal();
                int newMaxLoudness = MinMaxLoudnessValue;
                if (presenter.MaxLoudness >= MinMaxLoudnessValue)
                {
                    newMaxLoudness = presenter.MaxLoudness;
                }

                view.MaxLoudness = newMaxLoudness;
                SetMaxLoudness(newMaxLoudness);
            }
        }

        public void Dispose()
        {
            view.MaxVolumeChanged -= view_MaxVolumeChanged;
            view.MaxLoudnessChanged -= view_MaxLoudnessChanged;
            view.CultureCodeChanged -= view_CultureCodeChanged;
            view.AutoDetectLoudnessClick -= view_AutoDelectLoudnessClick;
            view.Closed -= view_Closed;
            volumeStatusTimer.Tick -= volumeStatusTimer_Tick;
        }
    }
}