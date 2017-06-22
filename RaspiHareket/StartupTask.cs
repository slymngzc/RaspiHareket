using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using MovementPin;
using System.Threading;
using Windows.System.Threading;
using Windows.Devices.Gpio;
using System.Threading.Tasks;
using System.Diagnostics;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace RaspiHareket
{
    public sealed class StartupTask : IBackgroundTask
    {
        //400 Milisaniye Olarak Ayarlıyorum************
        const double sec = 600;
        //*********************************************
        ThreadPoolTimer timerHareket = null;
        Movement movement = new Movement();
        ReadSensor readSensor = new ReadSensor();

        BackgroundTaskDeferral _deferral = null;

        //************Yakınlık Sensoru Verileri*****************
        private const int ECHO_PIN = 23;
        private const int TRIGGER_PIN = 18;
        private GpioPin pinEcho;
        private GpioPin pinTrigger;
        private ThreadPoolTimer timerSensor;
        private Stopwatch sw;
        //**********************************************

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            //movement.SagMotorOnYon();
            //movement.SolMotorOnYon();
            //this.timerHareket = ThreadPoolTimer.CreatePeriodicTimer(TimerHareket_Tick, TimeSpan.FromMilliseconds(sec));
            this.timerSensor = ThreadPoolTimer.CreatePeriodicTimer(TimerYakinlik_Tick, TimeSpan.FromMilliseconds(sec));
        }

        private void TimerYakinlik_Tick(ThreadPoolTimer timer)
        {
            pinTrigger.Write(GpioPinValue.High);
            Task.Delay(10);
            pinTrigger.Write(GpioPinValue.Low);
            sw.Start();
            while (pinEcho.Read() == GpioPinValue.Low)
            {

            }

            while (pinEcho.Read() == GpioPinValue.High)
            {
            }
            sw.Stop();

            var elapsed = sw.Elapsed.TotalSeconds;
            var distance = elapsed * 34000;

            distance /= 2;

        }

        /// <summary>
        /// Ana İşleri Yaptığım Kısım
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerHareket_Tick(ThreadPoolTimer timer)
        {
            string sensorValue = readSensor.GetSensorValue();


            //Hiç Çizgi Yoksa
            if (sensorValue == "000")
            {
                movement.SagMotorOnYon();
                movement.SolMotorOnYon();
            }
            //Soft Hareketler*****************
            else if (sensorValue == "001")
            {
                movement.SolMotorOnYon();
                movement.SagMotorDur();
            }
            else if (sensorValue == "010")
            {
                movement.SolMotorOnYon();
                movement.SagMotorOnYon();
            }
            else if (sensorValue == "100")
            {
                movement.SolMotorDur();
                movement.SagMotorOnYon();
            }
            //********************************
            //Keskin Hareketler
            else if (sensorValue == "011")
            {
                movement.SolMotorOnYon();
                movement.SagMotorArkaYon();
            }
            else if (sensorValue == "110")
            {
                movement.SolMotorArkaYon();
                movement.SagMotorOnYon();
            }
            else if (sensorValue == "111")
            {
                movement.SolMotorDur();
                movement.SagMotorDur();
            }
        }

        private async void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
            if (gpio == null)
            {
                pinEcho = null;
                pinTrigger = null;
                //gpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            pinEcho = gpio.OpenPin(ECHO_PIN);
            pinTrigger = gpio.OpenPin(TRIGGER_PIN);

            pinTrigger.SetDriveMode(GpioPinDriveMode.Output);
            pinEcho.SetDriveMode(GpioPinDriveMode.Input);
            //gpioStatus.Text = "GPIO controller and pins initialized successfully.";

            pinTrigger.Write(GpioPinValue.Low);

            await Task.Delay(100);
        }
    }
}
