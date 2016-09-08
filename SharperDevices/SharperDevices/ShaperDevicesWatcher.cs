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
using System.Threading;

namespace Extensions
{

    public static class ExtensionMethods
    {
        public static void ExtendedStop(this DeviceWatcher dw)
        {
            dw.Stop();
            WriteLine("Stopped");
        }
    }
}

namespace SharperDevices
{
    using Extensions;


    public class SharperDevicesWatcher
    {
        
        List<DeviceWatcher> ListOfActiveDeviceWatchers;
        Dictionary<DeviceWatcher, SharperDeviceWatcherInfo> DictSharperDeviceWatcherInfo;
        Dictionary<string, SharperDeviceInfo> DictOfSharperDeviceInfo;

        Timer WatcherRunTimer;
        AutoResetEvent WatcherAutoResetEvent;

        public SharperDevicesWatcher()
        {
            // 1. ListOfActiveDeviceWatchers holds references to all constructed watchers.
            // 2. DictOfDeviceTypes allows the DeviceType to be tracked (e.g., BLE, Classic, Wifi, etc.)
            // 3. DictOfSharperDeviceInfo allows device lookup based upon ID.

            ListOfActiveDeviceWatchers = new List<DeviceWatcher>();
            DictSharperDeviceWatcherInfo = new Dictionary<DeviceWatcher, SharperDeviceWatcherInfo>();
            DictOfSharperDeviceInfo = new Dictionary<string, SharperDeviceInfo>();      
        }

        public bool CreateWatcher(SharperDevice.DeviceTypes watcherType, Int32 watcherTimeout)
        {
            // 1. Create the watcher based on consumer choice.
            // 2. If constructed, add to dictionary and events.
            // 3. If creation was successful, return true.

            DeviceWatcher watcher = DeviceInformation.CreateWatcher(GetSelector(watcherType));
            if(watcher != null)
            {
                ListOfActiveDeviceWatchers.Add(watcher);
                SharperDeviceWatcherInfo watcherInfo = new SharperDeviceWatcherInfo(watcherType);
                DictSharperDeviceWatcherInfo[watcher] = watcherInfo;
                watcher.Added += Watcher_Added;
                watcher.Updated += Watcher_Updated;
                watcher.Stopped += Watcher_Stopped;
                watcher.Removed += Watcher_Removed;
                watcher.EnumerationCompleted += Watcher_EnumerationCompleted;
                WatcherRunTimer = new Timer(WatcherTimerExpired, null, watcherTimeout, Timeout.Infinite);
                return true;
            } else
            {
                return false;
            }
        }

        public bool CreateWatchers(List<SharperDevice.DeviceTypes> watcherTypes, Int32 watcherTimeout)
        {
            // 1. Create a list of watcher types based on consumer choice.

            foreach (var type in watcherTypes)
            {
                CreateWatcher(type, watcherTimeout);
            }
            return true;
        }

        private void WatcherTimerExpired(object obj)
        {
            foreach(var watcher in ListOfActiveDeviceWatchers)
            {
                if(watcher != null)
                {
                    switch (watcher.Status)
                    {
                        case DeviceWatcherStatus.Aborted:
                            break;
                        case DeviceWatcherStatus.Created:
                            break;
                        case DeviceWatcherStatus.EnumerationCompleted:
                            watcher.ExtendedStop();
                            break;
                        case DeviceWatcherStatus.Started:
                            break;
                        case DeviceWatcherStatus.Stopped:
                            break;
                        case DeviceWatcherStatus.Stopping:
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        private void Watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            
        }

        private void Watcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            WriteLine(GetSharperDeviceWatcherInfo(sender).WatcherType);
        }

        private void Watcher_Stopped(DeviceWatcher sender, object args)
        {
            SharperDeviceWatcherInfo senderInfo = GetSharperDeviceWatcherInfo(sender);
            if(true == senderInfo.Continuous)
            {
                sender.Start();
            }
        }

        private void Watcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            var deviceType = GetSharperDeviceWatcherInfo(sender);
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

            SharperDeviceInfo deviceInfo = new SharperDeviceInfo();
            var deviceType = GetSharperDeviceWatcherInfo(sender).WatcherType;
            if(!DictOfSharperDeviceInfo.ContainsKey(args.Id))
            {
                try
                {
                    PopulateSharperDeviceInfo(deviceInfo, args, deviceType);
                    DictOfSharperDeviceInfo[args.Id] = deviceInfo;
                    WriteLine($"Added device: {args.Name} -- DeviceType: {deviceType}");
                } catch (Exception ex)
                {
                    WriteLine($"Exception adding device: {args.Name} -- DeviceType: {deviceType}  Exception: {ex.Message}");
                    if (DictOfSharperDeviceInfo.ContainsKey(args.Id))
                    {
                        DictOfSharperDeviceInfo.Remove(args.Id);
                    }
                    
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
                watcher.ExtendedStop();
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

        public SharperDeviceWatcherInfo GetSharperDeviceWatcherInfo(DeviceWatcher deviceWatcher)
        {
            // 1. Get device type based on watcher.
            return DictSharperDeviceWatcherInfo[deviceWatcher];
        }

        public SharperDeviceInfo GetDeviceInfoByID(string ID)
        {
            return DictOfSharperDeviceInfo[ID];
        }

        private void PopulateSharperDeviceInfo(SharperDeviceInfo deviceInfo, DeviceInformation args, SharperDevice.DeviceTypes deviceType)
        {
            // 1. Populate device info, including type.
            // 2. If this is not a wireless device, do not populate pairing info.
            try
            {
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
            } catch (Exception ex)
            {
                WriteLine($"Exception adding device: {args.Name} -- DeviceType: {deviceType}");
            }

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
