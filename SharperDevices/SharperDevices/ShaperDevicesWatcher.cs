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
        Dictionary<DeviceWatcher,SharperDevice.DeviceTypes> DictOfDeviceTypes;

        Dictionary<string, SharperDeviceInfo> DictOfSharperDeviceInfo;

        public SharperDevicesWatcher()
        {
            // 1. ListOfActiveDeviceWatchers holds references to all constructed watchers.
            // 2. DictOfDeviceTypes allows the DeviceType to be tracked (e.g., BLE, Classic, Wifi, etc.)
            // 3. DictOfSharperDeviceInfo allows device lookup based upon ID.

            ListOfActiveDeviceWatchers = new List<DeviceWatcher>();
            DictOfDeviceTypes = new Dictionary<DeviceWatcher, SharperDevice.DeviceTypes>();
            DictOfSharperDeviceInfo = new Dictionary<string, SharperDeviceInfo>();
        }

        public bool CreateWatcher(SharperDevice.DeviceTypes watcherType)
        {
            // 1. Create the watcher based on consumer choice.
            // 2. If constructed, add to dictionary and events.
            // 3. If creation was successful, return true.

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

        public bool CreateWatchers(List<SharperDevice.DeviceTypes> watcherTypes)
        {
            // 1. Create a list of watcher types based on consumer choice.

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
            var deviceType = GetDeviceType(sender);
            if (DictOfSharperDeviceInfo.ContainsKey(args.Id))
            {
                SharperDeviceInfo deviceInfo = new SharperDeviceInfo();
                UpdateSharperDeviceInfo(deviceInfo, args);
                DictOfSharperDeviceInfo[args.Id] = deviceInfo;
            }
        }

        private void Watcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            // 1. Get SharperDeviceInfo based upon args.ID
            // 2. If SharperDeviceInfo is null, create it.
            // 3. Populate the SharperDeviceInfo fields.

            var deviceType = GetDeviceType(sender);
            if(!DictOfSharperDeviceInfo.ContainsKey(args.Id))
            {
                try
                {
                    SharperDeviceInfo deviceInfo = new SharperDeviceInfo();
                    PopulateSharperDeviceInfo(deviceInfo, args, deviceType);
                    DictOfSharperDeviceInfo[args.Id] = deviceInfo;
                    WriteLine($"Added device: {args.Name} -- DeviceType: {deviceType}");
                } catch (Exception ex)
                {
                    WriteLine($"Exception adding device: {args.Name} -- DeviceType: {deviceType}");
                }

            }
        }

        public void Start()
        {
            // 1. Start all created watchers.

            foreach(var watcher in ListOfActiveDeviceWatchers)
            {
                watcher.Start();
            }
        }

        public void Stop()
        {
            // 1. Stop all created watchers.

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

         public string GetSelector(SharperDevice.DeviceTypes selector)
        {
            // 1. Selector constructor.

            switch (selector)
            {
                case SharperDevice.DeviceTypes.USBtoUART:
                    return SerialDevice.GetDeviceSelector();
                    break;
                case SharperDevice.DeviceTypes.BluetoothClassic:
                    return BluetoothDevice.GetDeviceSelector();
                    break;
                case SharperDevice.DeviceTypes.BluetoothLE:
                    return BluetoothLEDevice.GetDeviceSelector();
                    break;
                case SharperDevice.DeviceTypes.WiFiDirect:
                    return WiFiDirectDevice.GetDeviceSelector();
                    break;
                default:
                    return null;
                    break;
            }
        }

        public SharperDevice.DeviceTypes GetDeviceType(DeviceWatcher deviceWatcher)
        {
            // 1. Get device type based on watcher.
            return DictOfDeviceTypes[deviceWatcher];
        }

        public SharperDeviceInfo GetDeviceInfoByID(string ID)
        {
            return DictOfSharperDeviceInfo[ID];
        }

        private void PopulateSharperDeviceInfo(SharperDeviceInfo deviceInfo, DeviceInformation args, SharperDevice.DeviceTypes deviceType)
        {
            // 1. Populate device info, including type.
            // 2. If this is not a wireless device, do not populate pairing info.

            deviceInfo.DeviceType = deviceType;
            deviceInfo.ID = args.Id;
            deviceInfo.Name = args.Name;
            deviceInfo.IsEnabled = args.IsEnabled;
            deviceInfo.IsDefault = args.IsDefault;
            if (args.Pairing != null)
            {
                deviceInfo.IsPaired = args.Pairing.IsPaired;
                deviceInfo.CanPair = args.Pairing.CanPair;
                deviceInfo.ProtectionLevel = args.Pairing.ProtectionLevel;
            }
            deviceInfo.Properties = args.Properties as Dictionary<string, object> ?? new Dictionary<string, object>();
            deviceInfo.Kind = args.Kind;
        }

        private void UpdateSharperDeviceInfo(SharperDeviceInfo deviceInfo, DeviceInformationUpdate args)
        {
            // 1. Populate device info, including type.
            // 2. If this is not a wireless device, do not populate pairing info.

            deviceInfo.ID = args.Id;
            deviceInfo.Properties = args.Properties as Dictionary<string, object> ?? new Dictionary<string, object>();
            deviceInfo.Kind = args.Kind;
        }

        public Dictionary<string, SharperDeviceInfo> GetListOfFoundDevices()
        {
            return DictOfSharperDeviceInfo;
        }
    }
}
