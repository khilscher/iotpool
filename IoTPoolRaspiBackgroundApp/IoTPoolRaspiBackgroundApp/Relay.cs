using Windows.Devices.Gpio;

namespace IoTPoolRaspiBackgroundApp
{
    class Relay
    {
        private bool powerOn;
        private const int LED_PIN = 5;
        private GpioPin pin;

        public Relay()
        {
            pin = GpioController.GetDefault().OpenPin(LED_PIN);
            pin.Write(GpioPinValue.High);
            pin.SetDriveMode(GpioPinDriveMode.Output);
            powerOn = false;
        }

        public void PowerOn()
        {
            pin.Write(GpioPinValue.Low);
            powerOn = true;
        }

        public void PowerOff()
        {
            pin.Write(GpioPinValue.High);
            powerOn = false;
        }
        public bool IsPowerOn
        {
            get
            {
                return powerOn;
            }
        }
    }
}
