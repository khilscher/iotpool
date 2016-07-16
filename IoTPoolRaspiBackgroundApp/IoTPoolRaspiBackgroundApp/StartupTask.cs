using Windows.ApplicationModel.Background;



// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace IoTPoolRaspiBackgroundApp
{
    public sealed class StartupTask : IBackgroundTask
    {

        //Replace with your device connection string. Use Azure Device Explorer to obtain this
        //https://github.com/Azure/azure-iot-sdks/blob/master/tools/DeviceExplorer/doc/how_to_use_device_explorer.md
        //Format: "HostName=<iothubname>.azure-devices.net;DeviceId=<devicename>;SharedAccessKey=<key>"
        static string iotPoolConnString = "";

        //Case sensitive device name of this device registered in IoT Hub. Should be apart of the iotPoolConnString above
        static string deviceName = "iotpool"; 

        BackgroundTaskDeferral deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();

            Processing pool = new Processing(iotPoolConnString, deviceName);

            pool.SendD2CMessagesAsync();

            pool.ReceiveC2DMessagesAsync();         

        }
    }
}
