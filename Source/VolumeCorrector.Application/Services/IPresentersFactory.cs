using VolumeCorrector.Application.Presenters;

namespace VolumeCorrector.Application.Services
{
    public interface IPresentersFactory
    {
        T CreatePresenter<T>() where T : IPresenter;
    }
}