using VolumeCorrector.Properties;

namespace VolumeCorrector.Model.ProgramSettings
{
    public class SettingsManager : ISettingsManager
    {
        public int MaxVolume
        {
            get => Settings.Default.MaxVolume;
            set => Settings.Default.MaxVolume = value;
        }

        public int MaxLoudness
        {
            get => Settings.Default.MaxLoudness;
            set => Settings.Default.MaxLoudness = value;
        }

        public string LanguageCode
        {
            get => Settings.Default.LanguageCode;
            set => Settings.Default.LanguageCode = value;
        }

        public bool Enabled
        {
            get => Settings.Default.Enabled;
            set => Settings.Default.Enabled = value;
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }
}