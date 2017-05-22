using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace MovementPin
{
    public class Movement
    {
        //Sol Motur pinleri****************************
        const int solPlusPin = 22;
        BasePin SolPlus = null;
        const int solMinusPin = 25;
        BasePin SolMinus = null;
        //*********************************************

        //Sağ motor pinleri****************************
        const int sagPlusPin = 16;
        BasePin SagPlus = null;
        const int sagMinusPin = 26;
        BasePin SagMinus = null;
        //*********************************************

        /// <summary>
        /// pinlerin ilklendiği yer
        /// </summary>
        public Movement()
        {
            SolPlus = new BasePin(solPlusPin, GpioPinDriveMode.Output);
            SolMinus = new BasePin(solMinusPin, GpioPinDriveMode.Output);

            SagPlus = new BasePin(sagPlusPin, GpioPinDriveMode.Output);
            SagMinus = new BasePin(sagMinusPin, GpioPinDriveMode.Output);
        }

        public bool SolMotorOnYon()
        {
            SolPlus.SetPinValue(GpioPinValue.High);
            SolMinus.SetPinValue(GpioPinValue.Low);
            return true;
        }

        public bool SolMotorArkaYon()
        {
            SolPlus.SetPinValue(GpioPinValue.Low);
            SolMinus.SetPinValue(GpioPinValue.High);
            return true;
        }

        public bool SolMotorDur()
        {
            SolPlus.SetPinValue(GpioPinValue.Low);
            SolMinus.SetPinValue(GpioPinValue.Low);
            return true;
        }

        public bool SagMotorOnYon()
        {
            SagPlus.SetPinValue(GpioPinValue.High);
            SagMinus.SetPinValue(GpioPinValue.Low);
            return true;
        }

        public bool SagMotorArkaYon()
        {
            SagPlus.SetPinValue(GpioPinValue.Low);
            SagMinus.SetPinValue(GpioPinValue.High);
            return true;
        }

        public bool SagMotorDur()
        {
            SagPlus.SetPinValue(GpioPinValue.Low);
            SagMinus.SetPinValue(GpioPinValue.Low);
            return true;
        }
    }
}
