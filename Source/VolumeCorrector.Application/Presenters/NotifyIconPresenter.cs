using System;
using VolumeCorrector.Application.Services;
using VolumeCorrector.Application.Views;
using VolumeCorrector.Core;

namespace VolumeCorrector.Application.Presenters
{
    public class NotifyIconPresenter : INotifyIconPresenter
    {
        private readonly INotifyIconView _view;
        private readonly IVolumeMonitor _volumeMonitor;
        private readonly ISettingsService _settings;
        private readonly IApplicationService _applicationService;
        private readonly IPresentersFactory _presentersFactory;

        private IOptionsPresenter _optionsPresenter;

        public NotifyIconPresenter(
            INotifyIconView view, 
            IVolumeMonitor volumeMonitor, 
            ISettingsService settings, 
            IApplicationService applicationService, 
            IPresentersFactory presentersFactory)
        {
            _settings = settings;
            _applicationService = applicationService;
            _presentersFactory = presentersFactory;
            _volumeMonitor = volumeMonitor;

            _view = view;
            _view.ExitClick += view_ExitClick;
            _view.StartStopClick += view_NotifyIconClick;
            _view.OptionsClick += view_OptionsClick;

            _view.UpdateStatus(_volumeMonitor.Enabled);
        }

        private void view_OptionsClick(object sender, EventArgs args)
        {
            if (_optionsPresenter != null)
            {
                _optionsPresenter.BringToFront();
                return;
            }

            _optionsPresenter = _presentersFactory.CreatePresenter<IOptionsPresenter>();
            _optionsPresenter.Closed += optionsPresenter_Closed;
            _optionsPresenter.Run();
        }

        private void optionsPresenter_Closed(object sender, EventArgs args)
        {
            _optionsPresenter.Closed -= optionsPresenter_Closed;
            _optionsPresenter.Dispose();
            _optionsPresenter = null;
        }

        private void view_ExitClick(object sender, EventArgs args)
        {
            _applicationService.Exit();
        }

        private void view_NotifyIconClick(object sender, EventArgs args)
        {
            if (_volumeMonitor.Enabled)
            {
                _volumeMonitor.Stop();
            }
            else
            {
                _volumeMonitor.Start();
            }

            _settings.Enabled = _volumeMonitor.Enabled;
            _settings.Save();

            _view.UpdateStatus(_volumeMonitor.Enabled);
        }

        public void Dispose()
        {
            _view.ExitClick -= view_ExitClick;
            _view.StartStopClick -= view_NotifyIconClick;
            _view.OptionsClick -= view_OptionsClick;

            _view.Dispose();
        }
    }
}