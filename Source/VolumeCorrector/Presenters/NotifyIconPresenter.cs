using System;
using System.Windows.Forms;
using Gvin.Injection;
using VolumeCorrector.Model.VolumeCorrection;
using VolumeCorrector.Properties;
using VolumeCorrector.Views;

namespace VolumeCorrector.Presenters
{
    public class NotifyIconPresenter : INotifyIconPresenter
    {
        private readonly INotifyIconView view;
        private readonly IVolumeMonitor volumeMonitor;
        private readonly IInjector injector;

        private IOptionsPresenter optionsPresenter;

        public NotifyIconPresenter(INotifyIconView view, IVolumeMonitor volumeMonitor, IInjector injector)
        {
            this.injector = injector;
            this.volumeMonitor = volumeMonitor;

            this.view = view;
            this.view.ExitClick += view_ExitClick;
            this.view.StartStopClick += view_NotifyIconClick;
            this.view.OptionsClick += view_OptionsClick;

            this.view.UpdateStatus(this.volumeMonitor.Enabled);
        }

        private void view_OptionsClick(object sender, EventArgs args)
        {
            if (optionsPresenter != null)
            {
                optionsPresenter.BringToFront();
                return;
            }

            optionsPresenter = injector.Create<IOptionsPresenter>();
            optionsPresenter.Closed += optionsPresenter_Closed;
            optionsPresenter.Run();
        }

        private void optionsPresenter_Closed(object sender, EventArgs args)
        {
            optionsPresenter.Closed -= optionsPresenter_Closed;
            optionsPresenter.Dispose();
            optionsPresenter = null;
        }

        private void view_ExitClick(object sender, EventArgs args)
        {
            Application.Exit();
        }

        private void view_NotifyIconClick(object sender, EventArgs args)
        {
            if (volumeMonitor.Enabled)
            {
                volumeMonitor.Stop();
            }
            else
            {
                volumeMonitor.Start();
            }

            Settings.Default.Enabled = volumeMonitor.Enabled;
            Settings.Default.Save();

            view.UpdateStatus(volumeMonitor.Enabled);
        }

        public void Dispose()
        {
            view.ExitClick -= view_ExitClick;
            view.StartStopClick -= view_NotifyIconClick;
            view.OptionsClick -= view_OptionsClick;

            view.Dispose();
        }
    }
}