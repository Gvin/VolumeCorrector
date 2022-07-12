using System.Threading.Tasks;

namespace VolumeCorrector.Core
{
    public interface IVolumeService
    {
        bool Initialized { get; }

        float GetVolume();

        void SetVolume(float volume);

        float GetLoudness();

        Task InitializeAsync();

        Task UpdateAsync();
    }
}