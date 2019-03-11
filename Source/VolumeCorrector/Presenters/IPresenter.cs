using System.Windows.Forms;

namespace VolumeCorrector.Presenters
{
    public interface IPresenter
    {
        void Run();

        DialogResult RunModal();
    }
}