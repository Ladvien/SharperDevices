using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFiDirect;
using Windows.Devices.SerialCommunication;
using Windows.Devices.Bluetooth.Advertisement;
using static System.Diagnostics.Debug;

namespace SharperDevices
{
    public class SharperDevicesWatcher
    {
        
        List<DeviceWatcher> ListOfActiveDeviceWatchers;
        Dictionary<DeviceWatcher,WatcherSelectors> DictOfDeviceTypes;  

        //public string BluetoothLEAdvertisementSelector = BluetoothLEAdvertisement

        public SharperDevicesWatcher()
        {
            ListOfActiveDeviceWatchers = new List<DeviceWatcher>();
            DictOfDeviceTypes = new Dictionary<DeviceWatcher, WatcherSelectors>();
        }

        public bool CreateWatcher(WatcherSelectors watcherType)
        {
            DeviceWatcher watcher = DeviceInformation.CreateWatcher(GetSelector(watcherType));
            if(watcher != null)
            {
                ListOfActiveDeviceWatchers.Add(watcher);
                DictOfDeviceTypes[watcher] = watcherType;
                watcher.Added += Watcher_Added;
                watcher.Updated += Watcher_Updated;
                watcher.Stopped += Watcher_Stopped;
                watcher.Removed += Watcher_Removed;
                return true;
            } else
            {
                return false;
            }
        }

        public bool CreateWatchers(List<WatcherSelectors> watcherTypes)
        {
            foreach(var type in watcherTypes)
            {
                CreateWatcher(type);
            }
            return true;
        }

        private void Watcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            WriteLine(GetDeviceType(sender));
        }

        private void Watcher_Stopped(DeviceWatcher sender, object args)
        {
            WriteLine(GetDeviceType(sender));
        }

        private void Watcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            WriteLine(GetDeviceType(sender));
        }

        private void Watcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            WriteLine(GetDeviceType(sender));
        }

        public void Start()
        {
            foreach(var watcher in ListOfActiveDeviceWatchers)
            {
                watcher.Start();
            }
        }

        public void Stop()
        {
            foreach (var watcher in ListOfActiveDeviceWatchers)
            {
                watcher.Stop();
            }
        }

        public SharperDevice SubscribeToDevice(SharperDevice device)
        {
            return new SharperDevice();
        }

        public SharperDevice SubscribeToDeviceWithName(string deviceName)
        {
            return new SharperDevice();
        }

        public SharperDevice SubscribeToDeviceWithId(string deviceID)
        {
            return new SharperDevice();
        }

        public enum WatcherSelectors
        {
            USBtoUART,
            BluetoothClassic,
            BluetoothLE,
            WiFiDirect
        }

        public string GetSelector(WatcherSelectors selector)
        {
            switch (selector)
            {
                case WatcherSelectors.USBtoUART:
                    return SerialDevice.GetDeviceSelector();
                    break;
                case WatcherSelectors.BluetoothClassic:
                    return BluetoothDevice.GetDeviceSelector();
                    break;
                case WatcherSelectors.BluetoothLE:
                    return BluetoothLEDevice.GetDeviceSelector();
                case WatcherSelectors.WiFiDirect:
                    return WiFiDirectDevice.GetDeviceSelector();
                default:
                    return null;
                    break;
            }
        }

        public WatcherSelectors GetDeviceType(DeviceWatcher deviceWatcher)
        {
            return DictOfDeviceTypes[deviceWatcher];
        }
    }
}
