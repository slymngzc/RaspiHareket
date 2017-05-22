using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using MovementPin;
using System.Threading;
using Windows.System.Threading;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace RaspiHareket
{
    public sealed class StartupTask : IBackgroundTask
    {
        //Komut Satırı

        //400 Milisaniye Olarak Ayarlıyorum************
        const double sec = 600;
        //*********************************************
        ThreadPoolTimer timer = null;
        Movement movement = new Movement();
        ReadSensor readSensor = new ReadSensor();

        BackgroundTaskDeferral _deferral = null;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            //movement.SagMotorOnYon();
            //movement.SolMotorOnYon();
            this.timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromMilliseconds(sec));
        }

        /// <summary>
        /// Ana İşleri Yaptığım Kısım
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(ThreadPoolTimer timer)
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
    }
}
