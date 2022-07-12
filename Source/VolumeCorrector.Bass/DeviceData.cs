using ManagedBass;

namespace VolumeCorrector.Bass;

public class DeviceData
{
    public DeviceInfo Info { get; init; }

    public int Id { get; init; }

    public bool Equals(DeviceData other)
    {
        return other.Info.Name == Info.Name && other.Info.Driver == Info.Driver;
    }
}
