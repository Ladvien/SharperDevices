﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using static System.Diagnostics.Debug;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SharperDevices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        DeviceWatcher BLEDeviceWatcher;
        SharperDevicesWatcher sdWatcher;
        SharperDeviceWatcherInfo sdWatcherInfo;

        public MainPage()
        {
            this.InitializeComponent();

            sdWatcher = new SharperDevicesWatcher();

            // 1. Create a list of Watcher Types
            // 2. Have SharperDevice create watchers for list of device types.
            // 3. Start all watchers.

            List<SharperDevice.DeviceTypes> watcherTypes = new List<SharperDevice.DeviceTypes>();
            watcherTypes.Add(SharperDevice.DeviceTypes.BluetoothLE);
            watcherTypes.Add(SharperDevice.DeviceTypes.WiFiDirect);
            watcherTypes.Add(SharperDevice.DeviceTypes.BluetoothClassic);
            watcherTypes.Add(SharperDevice.DeviceTypes.USBtoUART);
            sdWatcher.CreateWatchers(watcherTypes, 9000);
            sdWatcher.Start();
            Timer(19000);
            var dictOfFoundDevices = sdWatcher.GetListOfFoundDevices();
            WriteLine(dictOfFoundDevices.Count);


        }

        public async void Timer(Int16 time)
        {
            await Task.Delay(time);
        }
    }
}
