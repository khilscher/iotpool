using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;


namespace IoTPoolRaspiBackgroundApp
{
    class Processing
    {
        //Ensure this name (case sensitive) matches the device name registered in IoT Hub.
        //https://github.com/Azure/azure-iot-sdks/blob/master/tools/DeviceExplorer/doc/how_to_use_device_explorer.md
        string poolName = ""; 
        static DeviceClient deviceClient;
        Relay pool;
        Timer t;

        public Processing(string deviceConnString, string deviceName)
        {
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnString, TransportType.Amqp);
            pool = new Relay();
            t = new Timer(TimerCallback, null, 0, 30000); //Every 30 seconds
            Temperature.InitSensors();
            poolName = deviceName;
        }

        public async void SendD2CMessagesAsync()
        {

            await Task.Delay(10000); //Wait 10s for the sensors to initialize and receive data over I2C

            while (true)
            {
                var telemetryDataPoint = new
                {
                    PoolName = poolName,
                    PoolWaterTempC = Convert.ToDouble(Temperature.PoolTemperature),
                    OutsideAirTempC = Convert.ToDouble(Temperature.OutsideTemperature),
                    IsPoolPowerOn = pool.IsPowerOn,
                    SampleDateTime = DateTime.UtcNow
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);

                await Task.Delay(60000); //Send every 1 minute

            }         
        }

        public async void SendD2CMessageNowAsync()
        {
            var telemetryDataPoint = new
            {
                PoolName = poolName,
                PoolWaterTempC = Convert.ToDouble(Temperature.PoolTemperature),
                OutsideAirTempC = Convert.ToDouble(Temperature.OutsideTemperature),
                IsPoolPowerOn = pool.IsPowerOn,
                SampleDateTime = DateTime.UtcNow
            };
            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);

        }

        public async void ReceiveC2DMessagesAsync()
        {
            while (true)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage == null) continue;

                string onOffValue = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                //Delete the C2D message so we don't keep reprocessing it.
                await deviceClient.CompleteAsync(receivedMessage); 

                //Turn power on
                if (onOffValue == "1")
                {
                    pool.PowerOn();
                    SendD2CMessageNowAsync();
                }

                //Turn power off
                if (onOffValue == "0")
                {
                    pool.PowerOff();
                    SendD2CMessageNowAsync();
                }


            }              
        }

        private void TimerCallback(Object o)
        {
            
            TimeSpan onStart = new TimeSpan(20, 0, 0); //Start at 8pm local
            TimeSpan onDuration = TimeSpan.FromHours(3); //Run for 3 hours
            TimeSpan onEnd = onStart.Add(onDuration);
            TimeSpan now = DateTime.Now.TimeOfDay;
            

            /* For testing
            TimeSpan onStart = new TimeSpan(12, 25, 0); //Start at 9pm local
            TimeSpan onDuration = TimeSpan.FromMinutes(2); //2 min test cycle
            TimeSpan onEnd = onStart.Add(onDuration);
            TimeSpan now = DateTime.Now.TimeOfDay;
            */

            if ((now > onStart) && (now < onEnd))
            {
                pool.PowerOn();
                SendD2CMessageNowAsync();
            }

            TimeSpan offDuration = TimeSpan.FromMinutes(5); //5 minutes cycle to ensure pool is off
            TimeSpan offStart = onEnd.Add(offDuration);

            if ((now > onEnd) && (now < offStart))
            {
                pool.PowerOff();
                SendD2CMessageNowAsync();
            }
        }
    }
}
