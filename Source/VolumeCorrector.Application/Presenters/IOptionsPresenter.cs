using System;

namespace VolumeCorrector.Application.Presenters
{
    public interface IOptionsPresenter : IPresenter, IDisposable
    {
        void Run();

        event EventHandler Closed;

        void BringToFront();
    }
}