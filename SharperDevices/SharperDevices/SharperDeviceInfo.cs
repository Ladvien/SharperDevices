using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace SharperDevices
{
    public class SharperDeviceInfo
    {
        public SharperDevice.DeviceTypes DeviceType;
        public string ID;
        public string Name;
        public bool CanPair = false;
        public bool IsPaired = false;
        public DevicePairingProtectionLevel ProtectionLevel = DevicePairingProtectionLevel.Default;
        public DeviceInformationKind Kind;
        public Dictionary<string, object> Properties;
    }
}
