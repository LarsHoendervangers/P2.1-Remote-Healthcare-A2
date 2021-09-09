using Avans.TI.BLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Hardware
{
    interface IBLEDevice
    {
        Task<int> OpenDevice(string deviceName);
        Task<int> SetService(string serviceName);
        Task<int> SubscribeToCharacteristic(string param);
        //BLESubscriptionValueChangedEventHandler SubscriptionValueChanged;
        void OnDataReceived(object sender, BLESubscriptionValueChangedEventArgs e);

        void SetErrorCode(int errorcode);
        void SetConnectionAttempts(int connectionAttempts);
        int GetErrorCode();
        int GetConnectionAttempts();
    }
}
