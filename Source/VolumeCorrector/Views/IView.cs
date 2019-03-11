using System;
using System.Windows.Forms;

namespace VolumeCorrector.Views
{
    public interface IView
    {
        void Show();

        DialogResult ShowDialog();

        event EventHandler Closed;
    }
}