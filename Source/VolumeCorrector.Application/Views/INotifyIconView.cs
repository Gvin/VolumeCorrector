using System;

namespace VolumeCorrector.Application.Views
{
    public interface INotifyIconView : IDisposable
    {
        event EventHandler ExitClick;

        event EventHandler StartStopClick;

        event EventHandler OptionsClick;

        void UpdateStatus(bool enabled);
    }
}