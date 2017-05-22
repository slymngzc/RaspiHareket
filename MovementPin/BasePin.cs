using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace MovementPin
{
    public class BasePin
    {
        //Pini belirleyemedim.
        GpioController gpio = null;
        GpioPin pin = null;

        public BasePin(int pinAdi, GpioPinDriveMode pinDriveMode)
        {
            gpio = GpioController.GetDefault();
            if (gpio == null)
            {
                return;
            }

            pin = gpio.OpenPin(pinAdi);

            if (pinDriveMode == GpioPinDriveMode.Output)
            {
                pin.Write(GpioPinValue.Low);
            }
            pin.SetDriveMode(pinDriveMode);
        }

        public bool SetPinValue(GpioPinValue highOrLow)
        {
            pin.Write(highOrLow);
            return true;
        }

        public GpioPinValue GetPinValue()
        {
            return pin.Read();
        }
    }
}
