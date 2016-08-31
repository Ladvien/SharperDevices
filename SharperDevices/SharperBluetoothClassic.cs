using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.System;
using Windows.UI.Core;
using static System.Diagnostics.Debug;

namespace SharperDevices
{


    public class SharperBluetoothClassic: SharperDevices, ISharperDevices
    {
        DeviceWatcher BluetoothClassicWatcher;

        public SharperBluetoothClassic()
        {
            var BluetoothClassicSelector = BluetoothDevice.GetDeviceSelector();
            DeviceWatcher BluetoothClassicWatcher = DeviceInformation.CreateWatcher(BluetoothClassicSelector);
            BluetoothClassicWatcher.Added += BluetoothClassicWatcher_Added;
            BluetoothClassicWatcher.Removed += BluetoothClassicWatcher_Removed;
            BluetoothClassicWatcher.Updated += BluetoothClassicWatcher_Updated;
            BluetoothClassicWatcher.Stopped += BluetoothClassicWatcher_Stopped;
            BluetoothClassicWatcher.EnumerationCompleted += BluetoothClassicWatcher_EnumerationCompleted;
            BluetoothClassicWatcher.Start();
        }

        private void BluetoothClassicWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {

        }

        private void BluetoothClassicWatcher_Stopped(DeviceWatcher sender, object args)
        {

        }

        private void BluetoothClassicWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {

        }

        private void BluetoothClassicWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {

        }

        private async void BluetoothClassicWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            // 1. Print out all the Classic Bluetooth Device info.
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {

                WriteLine("####### Start BluetoothBLE Device Found ###########################################################");
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
                WriteLine("####### Start BluetoothBLE Device Found ###########################################################");
            });
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
