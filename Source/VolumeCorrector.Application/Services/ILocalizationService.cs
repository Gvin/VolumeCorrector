namespace VolumeCorrector.Application.Services
{
    public interface ILocalizationService
    {
        public string LanguageChangeText { get; }

        public string LanguageChangeCaption { get; }
    }
}