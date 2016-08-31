using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SharperDevices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SharperDevices deviceFactory;
        List<SharperDevices> sharperDevices;



        public MainPage()
        {
            this.InitializeComponent();
            sharperDevices = new List<SharperDevices>();

            SharperDevicesMaker sdFactory = new SharperDevicesMaker();

            SharperBluetoothClassic btClassicDevice = sdFactory.SharperDevicesFactory(SharperDevicesMaker.DeviceType.BluetoothClassic) as SharperBluetoothClassic;
            SharperBluetoothLE btLEDevice = sdFactory.SharperDevicesFactory(SharperDevicesMaker.DeviceType.BluetoothBLE) as SharperBluetoothLE;
            SharperWiFi usbUartDevice = sdFactory.SharperDevicesFactory(SharperDevicesMaker.DeviceType.WiFi) as SharperWiFi;
            sharperDevices.Add(btClassicDevice);
            sharperDevices.Add(btLEDevice);
            sharperDevices.Add(usbUartDevice);

        }


        private void BluetoothSettings_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (sd != null)
            {
                //sd.OpenBluetoothSettings();
            }
            else
            {

            }*/
        }

        private void WiFiSettings_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (sd != null)
            {
                sd.OpenWifiSettings();
            }
            else
            {

            }
            */
        }
    }
}
