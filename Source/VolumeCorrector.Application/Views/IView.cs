using System;

namespace VolumeCorrector.Application.Views
{
    public interface IView
    {
        void Show();

        event EventHandler Closed;
    }
}