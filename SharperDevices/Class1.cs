using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace SharperDevices
{
    class Class1
    {
        DeviceWatcher BLEDeviceWatcher;

        public Class1()
        {
            var selector = BluetoothLEDevice.GetDeviceSelector();
            BLEDeviceWatcher = DeviceInformation.CreateWatcher(selector);

            BLEDeviceWatcher.Added += BLEDeviceWatcher_Added;
            BLEDeviceWatcher.Removed += BLEDeviceWatcher_Removed;
            BLEDeviceWatcher.Updated += BLEDeviceWatcher_Updated;

            BLEDeviceWatcher.Start();

        }

        private void BLEDeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            throw new NotImplementedException();
        }

        private void BLEDeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            throw new NotImplementedException();
        }

        private void BLEDeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            throw new NotImplementedException();
        }
    }
}
