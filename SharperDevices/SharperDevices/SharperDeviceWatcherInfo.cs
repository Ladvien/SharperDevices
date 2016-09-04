using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Devices.WiFiDirect;
using static SharperDevices.SharperDevicesWatcher;

namespace SharperDevices
{

    public class SharperDeviceWatcherInfo
    {
        public SharperDevice.DeviceTypes WatcherType;
    }
}