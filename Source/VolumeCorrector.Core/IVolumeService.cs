using System.Threading.Tasks;

namespace VolumeCorrector.Core
{
    public interface IVolumeService
    {
        bool Initialized { get; }

        double GetVolume();

        void SetVolume(double volume);

        float GetLoudness();

        Task InitializeAsync();

        Task UpdateAsync();
    }
}