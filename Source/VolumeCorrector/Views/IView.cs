using System;
using System.Windows.Forms;
using Gvin.Injection;

namespace VolumeCorrector.Views
{
    public interface IView : IInjectable
    {
        void Show();

        DialogResult ShowDialog();

        event EventHandler Closed;
    }
}