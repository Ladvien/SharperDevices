using System;
using Windows.Devices.Bluetooth;
using static System.Diagnostics.Debug;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Threading;
using System.Collections.Generic;
using Windows.UI.Core;

namespace SharperDevices
{


    public class SharperBlue
    {

        //public delegate List<string> DiscoveredBLEDevice(List<string> deviceIDs);
        //public event DiscoveredBLEDevice FoundBLEDevice();

        private List<string> GattIDs;
        DeviceWatcher DeviceWatcher;
        List<UInt64> AddressesOfDiscoveredBLEAdvPackets = new List<UInt64>();
        BluetoothLEAdvertisementWatcher BLEAdvWatcher;


        public SharperBlue()
        {
            var selector = BluetoothLEDevice.GetDeviceSelector();
            DeviceWatcher = DeviceInformation.CreateWatcher(selector);
            DeviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            DeviceWatcher.Added += DeviceWatcher_Added;
            DeviceWatcher.Updated += DeviceWatcherUpdated;
            DeviceWatcher.Stopped += DeviceWatcher_Stopped;


            BluetoothLEAdvertisementWatcher BLEAdvWatcher = new BluetoothLEAdvertisementWatcher();

            BLEAdvWatcher.Received += BLEAdvWatcher_Received;
            BLEAdvWatcher.Stopped += BLEAdvWatcher_Stopped;

            BLEAdvWatcher.ScanningMode = BluetoothLEScanningMode.Active;

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
            () =>
            {
                try
                {
                    if (!AddressesOfDiscoveredBLEAdvPackets.Contains(args.BluetoothAddress))
                    {
                        AddressesOfDiscoveredBLEAdvPackets.Add(args.BluetoothAddress);
                        WriteLine($"BluetoothAddress:                  {args?.BluetoothAddress}");
                        WriteLine($"Count of ServiceUUID:              {args?.Advertisement?.ServiceUuids?.Count}");
                        WriteLine($"Advertisement type:                {args?.AdvertisementType}");
                        WriteLine($"RawSignalStrengthInDBm:            {args?.RawSignalStrengthInDBm}");
                        WriteLine($"Timestamp:                         {args?.Timestamp}");
                        WriteLine($"Advertisement DataSections Count:  {args?.Advertisement?.DataSections?.Count}");
                        WriteLine($"Advertisement Flags Value:         {args?.Advertisement?.Flags}");
                        WriteLine($"Advertisement LocalName:           {args?.Advertisement?.LocalName}");
                        WriteLine($"Advertisement Manf. Data Count:    {args?.Advertisement?.ManufacturerData.Count}");
                        WriteLine($"Advertisement ServiceUUIDs Count:  {args?.Advertisement?.ServiceUuids.Count}");
                        Write("\n\n");
                    }

                }
                catch (Exception ex)
                {
                    WriteLine($"Failed to open service.\nException: {ex.Message}");
                }
            });
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
