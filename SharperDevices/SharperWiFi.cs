using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFiDirect;
using static System.Diagnostics.Debug;

namespace SharperDevices
{
<<<<<<< Updated upstream:SharperDevices/SharperWiFi.cs
    class SharperWiFi: SharperDevices, ISharperDevices
=======
    class ShaperWiFi: ISharperDevices
>>>>>>> Stashed changes:SharperDevices/ShaperWiFi.cs
    {
        DeviceWatcher WiFiWatcher;

        public SharperWiFi()
        {
            var wifiSelector = WiFiDirectDevice.GetDeviceSelector();
            WiFiWatcher = DeviceInformation.CreateWatcher(wifiSelector);

            WiFiWatcher.Added += WiFiWatcher_Added;
            WiFiWatcher.EnumerationCompleted += WiFiWatcher_EnumerationCompleted;
            WiFiWatcher.Removed += WiFiWatcher_Removed;
            WiFiWatcher.Stopped += WiFiWatcher_Stopped;
            WiFiWatcher.Updated += WiFiWatcher_Updated;
            WiFiWatcher.Start();
        }

        private void WiFiWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            throw new NotImplementedException();
        }

        private void WiFiWatcher_Stopped(DeviceWatcher sender, object args)
        {
            throw new NotImplementedException();
        }

        private void WiFiWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            throw new NotImplementedException();
        }

        private void WiFiWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            WriteLine("Wifi enum comp-lete.");
        }

        private void WiFiWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void ClearDevices()
        {
            throw new NotImplementedException();
        }
    }
}
