using System;
using System.Globalization;
using System.Threading;
using VolumeCorrector.Application.Services;
using VolumeCorrector.Application.Views;
using VolumeCorrector.Core;

namespace VolumeCorrector.Application.Presenters
{
    public class OptionsPresenter : IOptionsPresenter
    {
        private readonly IOptionsView _view;
        private readonly IVolumeMonitor _volumeMonitor;
        private readonly ISettingsService _settings;
        private readonly IMessageBoxService _messageBoxService;
        private readonly ILocalizationService _localizationService;

        public event EventHandler Closed;

        public OptionsPresenter(
            IOptionsView view, 
            IVolumeMonitor volumeMonitor, 
            ISettingsService settings, 
            IMessageBoxService messageBoxService, 
            ILocalizationService localizationService)
        {
            _settings = settings;
            _messageBoxService = messageBoxService;
            _localizationService = localizationService;
            _view = view;
            _volumeMonitor = volumeMonitor;

            view.MaxVolume = volumeMonitor.MaxVolume;
            view.MaxLoudness = volumeMonitor.MaxLoudness;
            view.CultureCode = GetCurrentCultureCode();
            RefreshSoundMetrics();

            _view.MaxVolumeChanged += view_MaxVolumeChanged;
            _view.MaxLoudnessChanged += view_MaxLoudnessChanged;
            _view.CultureCodeChanged += view_CultureCodeChanged;
            _view.MetricsRefreshTimer += view_MetricsRefreshTimer;
            _view.Closed += view_Closed;
        }

        private void view_MetricsRefreshTimer(object sender, EventArgs e)
        {
            RefreshSoundMetrics();
        }

        private string GetCurrentCultureCode()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public void Run()
        {
            _view.Show();
        }

        public void BringToFront()
        {
            _view.ForceBringToFront();
        }

        private void RefreshSoundMetrics()
        {
            _view.Volume = _volumeMonitor.Volume;
            _view.Loudness = _volumeMonitor.Loudness;
        }

        private void view_Closed(object sender, EventArgs args)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void view_MaxVolumeChanged(object sender, EventArgs args)
        {
            _volumeMonitor.MaxVolume = _view.MaxVolume;
            _settings.MaxVolume = _view.MaxVolume;
            _settings.Save();
        }

        private void view_MaxLoudnessChanged(object sender, EventArgs args)
        {
            SetMaxLoudness(_view.MaxLoudness);
        }

        private void SetMaxLoudness(int value)
        {
            _volumeMonitor.MaxLoudness = value;
            _settings.MaxLoudness = value;
            _settings.Save();
        }

        private void view_CultureCodeChanged(object sender, EventArgs args)
        {
            _settings.LanguageCode = _view.CultureCode;
            _settings.Save();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_view.CultureCode);

            _messageBoxService.ShowWarning(_localizationService.LanguageChangeCaption, _localizationService.LanguageChangeText);
        }

        public void Dispose()
        {
            _view.MaxVolumeChanged -= view_MaxVolumeChanged;
            _view.MaxLoudnessChanged -= view_MaxLoudnessChanged;
            _view.CultureCodeChanged -= view_CultureCodeChanged;
            _view.MetricsRefreshTimer -= view_MetricsRefreshTimer;
            _view.Closed -= view_Closed;
        }
    }
}