using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharperDevices
{
    public class SharperDevice
    {
        public enum DeviceTypes
        {
            // 1. Supported device types.

            USBtoUART,
            BluetoothClassic,
            BluetoothLE,
            WiFiDirect
        }
    }
}
