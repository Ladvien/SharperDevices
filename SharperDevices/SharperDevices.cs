using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace SharperDevices
{
    public abstract class SharperDevices
    {

        public async void OpenBluetoothSettings()
        {
            bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:bluetooth"));
        }

        public async void OpenWifiSettings()
        {
            bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:network-wifi"));
        }
    }



}
