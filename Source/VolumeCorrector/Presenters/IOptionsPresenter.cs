using System;

namespace VolumeCorrector.Presenters
{
    public interface IOptionsPresenter : IPresenter, IDisposable
    {
        void Run();

        event EventHandler Closed;

        void BringToFront();
    }
}