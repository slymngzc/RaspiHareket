using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using MovementPin;
using System.Threading;
using Windows.System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.Diagnostics;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace RaspiHareket
{
    public sealed class StartupTask : IBackgroundTask
    {
        //Siyah Renk 1 değeri 
        //Beyaz Renk 0 değerini ifade ediyor.
        //100 Milisaniye Olarak Ayarlıyorum************
        //Kullanılan PINler 
        //Sensörden gelen 3 bitlik veri (5 6 13 pinleri) Sol motor için 22 25 Sağ motor için 16 26
        //const double sec = 100; //Değişiklikleri algılama süresi
        //const int ileribeklemesuresisec = 30; // Rakam küçüldükçe ilermeme hızı düşer
        //const int softdonusbeklemesuresisec = 20; //Dönüşlerde diğer motorun ters yönde devreye girme süresi bekletme
        //*********************************************
        ThreadPoolTimer timer = null;
        Movement movement = new Movement();
        ReadSensor readSensor = new ReadSensor();

        BackgroundTaskDeferral _deferral = null;

        //Mesafe Sensörü Bilgileri
        private const int ECHO_PIN = 23;
        private const int TRIGGER_PIN = 18;
        private GpioPin pinEcho;
        private GpioPin pinTrigger;
        private Stopwatch sw;

        //Web Servis Kısmı
        RaspiWebService.RepositoryClient repoClient = null;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            sw = new Stopwatch();
            repoClient = new RaspiWebService.RepositoryClient();
            _deferral = taskInstance.GetDeferral();
            InitGPIO();
            if (pinEcho != null & pinTrigger != null)
            {
                var result = repoClient.AddValueAsync("INFO", "RUN METODU İÇİNDEYİZ").Result;
                this.timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromMilliseconds(1000));
            }
        }

        /// <summary>
        /// Ana İşleri Yaptığım Kısım
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //    private void Timer_Tick(ThreadPoolTimer timer)
        //    {
        //        string sensorValue = readSensor.GetSensorValue();
        //        switch (sensorValue)
        //        {
        //            case "000": //Hiç Çizgi Yoksa Çift Motor ileri
        //                movement.SagMotorOnYon();
        //                movement.SolMotorOnYon();
        //                Task.Delay(-1).Wait(ileribeklemesuresisec);
        //                movement.SagMotorDur();
        //                movement.SolMotorDur();
        //                break;
        //            case "010":  //Ortada çizgi var ise Çift Motor ileri
        //                movement.SolMotorOnYon();
        //                movement.SagMotorOnYon();
        //                Task.Delay(-1).Wait(ileribeklemesuresisec);
        //                movement.SolMotorDur();
        //                movement.SagMotorDur();
        //                break;
        //            case "001": //Sola dönüş çizgileri takip et 
        //                movement.SolMotorDur();
        //                movement.SagMotorDur();
        //                Task.Delay(-1).Wait(softdonusbeklemesuresisec);
        //                movement.SolMotorOnYon();
        //                movement.SagMotorArkaYon();
        //                break;
        //            case "100": //Sağa dönüş
        //                movement.SolMotorDur();
        //                movement.SagMotorDur();
        //                Task.Delay(-1).Wait(softdonusbeklemesuresisec);
        //                movement.SagMotorOnYon();
        //                movement.SolMotorArkaYon();
        //                break;
        //            case "011": //Sağa keskin dönüş
        //                movement.SolMotorOnYon();
        //                movement.SagMotorArkaYon();
        //                break;
        //            case "110": //Sola keskin dönüş
        //                movement.SolMotorArkaYon();
        //                movement.SagMotorOnYon();
        //                break;
        //            case "111":
        //                movement.SolMotorDur();
        //                movement.SagMotorDur();
        //                break;
        //            default:
        //                movement.SolMotorDur();
        //                movement.SagMotorDur();
        //                break;
        //        }
        //    //}

        private async void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
            if (gpio == null)
            {
                pinEcho = null;
                pinTrigger = null;
                await repoClient.AddValueAsync("INFO", "There is no GPIO controller on this device");
                return;
            }

            pinEcho = gpio.OpenPin(ECHO_PIN);
            pinTrigger = gpio.OpenPin(TRIGGER_PIN);

            pinTrigger.SetDriveMode(GpioPinDriveMode.Output);
            pinEcho.SetDriveMode(GpioPinDriveMode.Input);
            await repoClient.AddValueAsync("INFO", "GPIO controller and pins initialized successfully.");

            pinTrigger.Write(GpioPinValue.Low);

            await Task.Delay(100);
        }

        private void Timer_Tick(ThreadPoolTimer timer)
        {
            pinTrigger.Write(GpioPinValue.High);
            Task.Delay(10);
            pinTrigger.Write(GpioPinValue.Low);
            sw.Start();
            while (pinEcho.Read() == GpioPinValue.Low)
            {
                sw.Restart();
            }

            while (pinEcho.Read() == GpioPinValue.High)
            {
            }
            sw.Stop();

            var elapsed = sw.Elapsed.TotalSeconds;
            var distance = elapsed * 34000;

            distance /= 2;
            repoClient.AddValueAsync("DISTANCE: ", distance + " cm");
        }
    }
}
