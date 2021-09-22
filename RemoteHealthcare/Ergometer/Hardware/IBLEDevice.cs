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
        /// <summary>
        /// Probably unnessecary
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        Task<int> OpenDevice(string deviceName);
        /// <summary>
        /// Probably unnessecary
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task<int> SetService(string serviceName);/// <summary>
        /// Probably unnessecary
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> SubscribeToCharacteristic(string param);
        /// <summary>
        /// Event used for sending data from 3rd layer to 2nd layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDataReceived(object sender, BLESubscriptionValueChangedEventArgs e);
        /// <summary>
        /// Error code registered in Device but must be sent to PhysicalDevice
        /// </summary>
        /// <param name="errorcode"></param>
        void SetErrorCode(int errorcode);
        /// <summary>
        /// Connection attempts registered in Device but must be sent to PhysicalDevice
        /// </summary>
        /// <param name="connectionAttempts"></param>
        void SetConnectionAttempts(int connectionAttempts);
        /// <summary>
        /// Error code registered in Device but must be sent to PhysicalDevice
        /// </summary>
        /// <returns></returns>
        int GetErrorCode();
        /// <summary>
        /// Connection attempts registered in Device but must be sent to PhysicalDevice
        /// </summary>
        /// <returns></returns>
        int GetConnectionAttempts();
    }
}
