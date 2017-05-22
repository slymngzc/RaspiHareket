using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace MovementPin
{
    public class ReadSensor
    {
        //sol sensör*****************
        const int solSensorPin = 5;
        BasePin SolSensor = null;
        //***************************

        //orta sensör****************
        const int ortaSensorPin = 6;
        BasePin OrtaSensor = null;
        //***************************

        //sag sensör*****************
        const int sagSensorPin = 13;
        BasePin SagSensor = null;
        //***************************

        public ReadSensor()
        {
            SolSensor = new BasePin(solSensorPin, GpioPinDriveMode.Input);
            OrtaSensor = new BasePin(ortaSensorPin, GpioPinDriveMode.Input);
            SagSensor = new BasePin(sagSensorPin, GpioPinDriveMode.Input);
        }

        public string GetSensorValue()
        {
            string temp = string.Empty;

            GpioPinValue sol = SolSensor.GetPinValue();
            temp = (sol == GpioPinValue.High ? "1" : "0");

            GpioPinValue orta = OrtaSensor.GetPinValue();
            temp += (orta == GpioPinValue.High ? "1" : "0");

            GpioPinValue sag = SagSensor.GetPinValue();
            temp += (sag == GpioPinValue.High ? "1" : "0");

            return temp;
        }
    }
}
