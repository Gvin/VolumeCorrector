using System;
using System.Drawing;

namespace VolumeCorrector.Views
{
    public interface INotifyIconView : IDisposable
    {
        event EventHandler ExitClick;

        event EventHandler StartStopClick;

        event EventHandler OptionsClick;

        void UpdateStatus(bool enabled);
    }
}