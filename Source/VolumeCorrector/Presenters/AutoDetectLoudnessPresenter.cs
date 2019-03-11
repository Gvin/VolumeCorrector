using System;
using System.Windows.Forms;
using VolumeCorrector.Views;

namespace VolumeCorrector.Presenters
{
    public class AutoDetectLoudnessPresenter : IDisposable, IPresenter
    {
        private readonly IAutoDetectLoudnessView view;

        public AutoDetectLoudnessPresenter(IAutoDetectLoudnessView view)
        {
            this.view = view;
        }

        public int MaxLoudness
        {
            get { return 100; }
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }

        public DialogResult RunModal()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}