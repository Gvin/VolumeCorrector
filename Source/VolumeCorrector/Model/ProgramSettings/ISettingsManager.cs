using Gvin.Injection;

namespace VolumeCorrector.Model.ProgramSettings
{
    /// <summary>
    /// Manages program settings.
    /// </summary>
    public interface ISettingsManager : IInjectable
    {
        /// <summary>
        /// Gets or sets configured max volume value.
        /// </summary>
        int MaxVolume { get; set; }

        /// <summary>
        /// Gets or sets configured max loudness value.
        /// </summary>
        int MaxLoudness { get; set; }

        /// <summary>
        /// Gets or sets currently selected user language.
        /// </summary>
        string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets if application should be enabled on start.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Saves all changed made in the configuration.
        /// </summary>
        void Save();
    }
}