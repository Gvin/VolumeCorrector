using System;

namespace VolumeCorrector.Views
{
    public interface IOptionsView : IView
    {
        int Volume { set; }

        int Loudness { set; }

        int MaxVolume { get; set; }

        int MaxLoudness { get; set; }

        string CultureCode { get; set; }

        event EventHandler MaxVolumeChanged;

        event EventHandler MaxLoudnessChanged;

        event EventHandler CultureCodeChanged;

        void ForceBringToFront();
    }
}