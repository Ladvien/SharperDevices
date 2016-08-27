using System;
using Windows.Devices.Bluetooth;
using static System.Diagnostics.Debug;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Threading;
using System.Collections.Generic;
using Windows.UI.Core;
using static Windows.Security.Cryptography.CryptographicBuffer;

namespace SharperDevices
{


    public class SharperBluetoothLE
    {

        DeviceWatcher BluetoothLEWatcher;

        public SharperBluetoothLE()
        {
            var BLESelector = BluetoothLEDevice.GetDeviceSelector();
            DeviceWatcher BLEWatcher = DeviceInformation.CreateWatcher(BLESelector);
            BLEWatcher.Added += BLEWatcher_Added;
            BLEWatcher.Removed += BLEWatcher_Removed;
            BLEWatcher.Updated += BLEWatcher_Updated;
            BLEWatcher.Stopped += BLEWatcher_Stopped;
            BLEWatcher.EnumerationCompleted += BLEWatcher_EnumerationCompleted;
            BLEWatcher.Start();

            var BluetoothSelector = BluetoothDevice.GetDeviceSelector();
            DeviceWatcher BluetoothWatcher = DeviceInformation.CreateWatcher(BluetoothSelector);
            BluetoothWatcher.Added += BLEWatcher_Added;
            BluetoothWatcher.Removed += BLEWatcher_Removed;
            BluetoothWatcher.Updated += BLEWatcher_Updated;
            BluetoothWatcher.Stopped += BLEWatcher_Stopped;
            BluetoothWatcher.EnumerationCompleted += BLEWatcher_EnumerationCompleted;
            BluetoothWatcher.Start();
        }

        private void BLEWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            WriteLine("Enum complete");
        }

        private void BLEWatcher_Stopped(DeviceWatcher sender, object args)
        {
            WriteLine("Stopped");
        }

        private void BLEWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            WriteLine("Updated");
        }

        private void BLEWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            WriteLine("Removed");
        }

        private void BLEWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            WriteLine("Added");
        }
    }

    public class SharperBlueAdvertisement
    {

        //public delegate List<string> DiscoveredBLEDevice(List<string> deviceIDs);
        //public event DiscoveredBLEDevice FoundBLEDevice();

        private List<string> GattIDs;
        DeviceWatcher DeviceWatcher;
        List<UInt64> AddressesOfDiscoveredBLEAdvPackets = new List<UInt64>();
        BluetoothLEAdvertisementWatcher BLEAdvWatcher;

        private List<BluetoothLEDevice> bluetoothLEDevices = new List<BluetoothLEDevice>();
        public void addDiscoveredBLEDeviceToList(BluetoothLEDevice device)
        {
            bluetoothLEDevices.Add(device);
        }

        public BluetoothLEDevice getDiscoveredBLEDeviceFromList(UInt16 deviceNumber)
        {
            if(bluetoothLEDevices[deviceNumber] != null)
            {
                return bluetoothLEDevices[deviceNumber];
            } else
            {
                return null;
            }
        }


        public SharperBlueAdvertisement()
        {
            BLEAdvWatcher = new BluetoothLEAdvertisementWatcher();
            //_BluetoothLEDevices = new List<BluetoothLEDevice>();

            var selector = BluetoothLEDevice.GetDeviceSelector();
            DeviceWatcher = DeviceInformation.CreateWatcher(selector);
            DeviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            DeviceWatcher.Added += DeviceWatcher_Added;
            DeviceWatcher.Updated += DeviceWatcherUpdated;
            DeviceWatcher.Stopped += DeviceWatcher_Stopped;

            BLEAdvWatcher.Received += BLEAdvWatcher_Received;
            BLEAdvWatcher.Stopped += BLEAdvWatcher_Stopped;

            BLEAdvWatcher.SignalStrengthFilter.SamplingInterval = new TimeSpan(0, 0, 0, 0, 1210); // Overrides other sampling properties?

            BLEAdvWatcher.ScanningMode = BluetoothLEScanningMode.Active;
            WriteLine("#####################################################################");
            WriteLine($"Starting BLE Adv Watcher...");
            WriteLine($"Status:                            {BLEAdvWatcher.Status}");
            WriteLine($"MinSamplingInterval:               {BLEAdvWatcher.MinSamplingInterval}");
            WriteLine($"MaxSamplingInterval:               {BLEAdvWatcher.MaxSamplingInterval}");
            WriteLine($"MinOutOfRangeTimeout:              {BLEAdvWatcher.MinOutOfRangeTimeout}");
            WriteLine($"MaxOutOfRangeTimeout:              {BLEAdvWatcher.MaxOutOfRangeTimeout}");
            WriteLine($"SamplingInterval:                  {BLEAdvWatcher.SignalStrengthFilter.SamplingInterval}");

            BLEAdvWatcher.Start();
            DeviceWatcher.Start();

            GattIDs = new List<string>();
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            throw new NotImplementedException();
        }

        private void BLEAdvWatcher_Stopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            sender.Start();
        }

        private async void BLEAdvWatcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                try
                {
                    if (!AddressesOfDiscoveredBLEAdvPackets.Contains(args.BluetoothAddress))
                    {
                        WriteLine("#####################################################################");
                        WriteLine($"Scanning Mode:                     {BLEAdvWatcher.ScanningMode}");
                        AddressesOfDiscoveredBLEAdvPackets.Add(args.BluetoothAddress);
                        WriteLine($"BluetoothAddress:                  {args?.BluetoothAddress}");
                        WriteLine($"Count of ServiceUUID:              {args?.Advertisement?.ServiceUuids?.Count}");
                        WriteLine($"Advertisement type:                {args?.AdvertisementType}");
                        WriteLine($"RawSignalStrengthInDBm:            {args?.RawSignalStrengthInDBm}");
                        WriteLine($"Timestamp:                         {args?.Timestamp}");
                        for(int i = 0; i < args?.Advertisement?.DataSections.Count;i++)
                        {
                            WriteLine($"Advertisement DataSections #{i}:     {EncodeToHexString(args?.Advertisement?.DataSections[i].Data)}");
                        }
                        WriteLine($"Advertisement Flags Value:         {args?.Advertisement?.Flags}");
                        WriteLine($"Advertisement LocalName:           {args?.Advertisement?.LocalName}");
                        for (int i = 0; i < args?.Advertisement?.ManufacturerData.Count; i++)
                        {
                            WriteLine($"Advertisement DataSections #{i}:     {EncodeToHexString(args?.Advertisement?.ManufacturerData[i].Data)}");
                        }
                        for (int i = 0; i < args?.Advertisement?.ServiceUuids.Count; i++)
                        {
                            WriteLine($"Advertisement ServiceUUIDs #{i}:     {args?.Advertisement?.ServiceUuids[i]}");
                        }
                        //if(args.Advertisement.ServiceUuids.Count > 0)
                        //{
                            WriteLine("Services of Device -----------------------------------------------");
                            var discoveredBLEDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
                            addDiscoveredBLEDeviceToList(discoveredBLEDevice);
                            WriteLine($"Device name:                       {getDiscoveredBLEDeviceFromList(0).DeviceInformation.Name}");                            
                        //}
                        WriteLine("#####################################################################");
                    }

                }
                catch (System.IO.FileNotFoundException ex)
                {
                    WriteLine($"Failed to open service.\nException: {ex.Message}");
                }
            });
        }

        private void DiscoveredBLEDevice_GattServicesChanged(BluetoothLEDevice sender, object args)
        {
            throw new NotImplementedException();
        }

        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {

        }

        private void DeviceWatcherUpdated(DeviceWatcher watcher, DeviceInformationUpdate update)
        {
            WriteLine($"Device updated: {update.Id}");
        }

        private void DeviceWatcher_Added(DeviceWatcher watcher, DeviceInformation device)
        {
            //if (_devices.Contains(device.Name))
            //{
            try
            {
                GattIDs.Add(device.Id);
                //WriteLine(device.Name);
                //var service = await GattDeviceService.FromIdAsync(device.Id);                   
            }
            catch (Exception exception)
            {
                WriteLine($"Failed to open service: {exception.Message}");
            }
            //}
        }

        public void StartBLEScan()
        {

        }

        public async void discoverServiceUUIDs(List<bool> services)
        {
            //foreach (var serviceID in services)
            //{
            //    var blah = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(serviceID));
            //    if (blah.Count > 0)
            //    {
            //        WriteLine("Do stuff");
            //    }
            //}
        }

    }

}
