using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharperDevices
{
    public class SharperDevicesMaker
    {
        public enum DeviceType
        {
            USBUART,
            BluetoothClassic,
            BluetoothBLE,
            WiFi
        }

        public SharperDevices SharperDevicesFactory(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.USBUART:
                    // USBUART
                    break;
                case DeviceType.BluetoothClassic:
                    return new SharperBluetoothClassic();
                    break;
                case DeviceType.BluetoothBLE:
                    return new SharperBluetoothLE();
                    break;
                case DeviceType.WiFi:
                    return new SharperWiFi();
                    break;
                default:
                    return null;
            }
            return null;
        }
    }
}
