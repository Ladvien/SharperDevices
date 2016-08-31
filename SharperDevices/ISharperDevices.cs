using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharperDevices
{
    interface ISharperDevices
    {

        void Connect();

        void Disconnect();

        void ClearDevices();

    }
}
