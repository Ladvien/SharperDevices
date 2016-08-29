using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using static System.Diagnostics.Debug;


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
            // 1. Print out all the Classic Bluetooth Device info.

            WriteLine("####### Start BluetoothClassic Device Found #######################################################");
            WriteLine($"        ID:                                                {args.Id}");
            WriteLine($"        Name:                                              {args.Name}");
            WriteLine($"        CanPair:                                           {args.Pairing.CanPair}");
            WriteLine($"        Pairing Protection Level:                          {args.Pairing.ProtectionLevel}");
            WriteLine($"        IsPaired:                                          {args.Pairing.IsPaired}");
            WriteLine($"        Kind:                                              {args.Kind}");
            for (int i = 0; i < args.Properties.Count; i++) { WriteLine($"        Properties #{i}:                                     {args.Properties.Values.ToArray()[i]}"); }
            WriteLine($"        EnclosureLocation.InDock:                          {args?.EnclosureLocation?.InDock}");
            WriteLine($"        EnclosureLocation.InLid:                           {args?.EnclosureLocation?.InLid}");
            WriteLine($"        EnclosureLocation.Panel:                           {args?.EnclosureLocation?.Panel}");
            WriteLine($"        EnclosureLocation.RotationAngleInDegreesClockwise: {args?.EnclosureLocation?.RotationAngleInDegreesClockwise}");
            WriteLine("####### End BluetoothClassic Device Found #########################################################");
        }
    }
}

