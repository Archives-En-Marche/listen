using System.Threading.Tasks;

namespace Listen.Services
{
    public interface IPermissions
    {
        Task RequestPermissionsAsync();
    }

    public enum PermissionStatus
    {
        // Denied by user
        Denied,

        // Feature is disabled on device
        Disabled,

        // Granted by user
        Granted,

        // Restricted (only iOS)
        Restricted,

        // Permission is in an unknown state
        Unknown
    }

    public enum PermissionType
    {
        Unknown,
        Battery,
        Camera,
        Flashlight,
        LocationWhenInUse,
        NetworkState,
        Vibrate,
        InternalStorage,
        RecordAudio,
    }
}
