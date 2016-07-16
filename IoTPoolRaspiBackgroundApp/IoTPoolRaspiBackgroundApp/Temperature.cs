using System;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

//Code sample from Mike Mackes https://create.arduino.cc/projecthub/mmackes/pool-controller-8dfa69

namespace IoTPoolRaspiBackgroundApp
{
    public static class Temperature
    {
        private static I2cDevice Device;
        private static Timer periodicTimer;
        //How often to read temperature data from the Arduino
        private static int ReadInterval = 4000;  //4000 = 4 seconds

        //Variables to hold temperature data
        private static string poolTemperature = "00.00";
        private static string outsideTemperature = "00.00";

        //Initilizes the I2C connection and starts the timer to read I2C Data
        async public static void InitSensors()
        {
            //Set up the I2C connection the Arduino
            var settings = new I2cConnectionSettings(0x40); // Arduino address
            settings.BusSpeed = I2cBusSpeed.StandardMode;
            string aqs = I2cDevice.GetDeviceSelector("I2C1");
            var dis = await DeviceInformation.FindAllAsync(aqs);
            Device = await I2cDevice.FromIdAsync(dis[0].Id, settings);

            //Create a timer to periodicly read the temps from the Arduino
            periodicTimer = new Timer(Temperature.TimerCallback, null, 0, ReadInterval);
        }

        //Property to expose the Temperature Data
        public static string PoolTemperature
        {
            get
            {   //Lock the variable incase the timer is tring to write to it
                lock (poolTemperature)
                {
                    return poolTemperature;
                }
            }

            set
            {   //Lock the variable incase the HTTP Server is tring to read from it
                lock (poolTemperature)
                {
                    poolTemperature = value;
                }
            }
        }

        //Property to expose the Temperature Data
        public static string OutsideTemperature
        {
            get
            {   //Lock the variable incase the timer is tring to write to it
                lock (outsideTemperature)
                {
                    return outsideTemperature;
                }
            }

            set
            {   //Lock the variable incase the HTTP Server is tring to read from it
                lock (outsideTemperature)
                {
                    outsideTemperature = value;
                }
            }
        }

        //Handle the time call back
        private static void TimerCallback(object state)
        {
            byte[] RegAddrBuf = new byte[] { 0x40 };
            byte[] ReadBuf = new byte[24];
            //Read the I2C connection
            try
            {
                Device.Read(ReadBuf); // read the data
            }
            catch (Exception) { }

            //Parse the response
            //Data is in the format "88.99|78.12" where "PoolTemp|OutsideTemp"
            char[] cArray = System.Text.Encoding.UTF8.GetString(ReadBuf, 0, 23).ToCharArray();  // Convert Byte to Char
            String c = new String(cArray).Trim();
            string[] data = c.Split('|');

            //Write the data to temperature variables
            try
            {
                if (data[0].Trim() != "")
                    PoolTemperature = data[0];
                if (data[1].Trim() != "")
                    OutsideTemperature = data[1];
            }
            catch (Exception) { }
        }
    }
}
