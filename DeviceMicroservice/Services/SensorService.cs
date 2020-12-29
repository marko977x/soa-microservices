using System;
using System.Timers;

namespace DeviceMicroservice.Services
{
    public class SensorService
    {
        public const float INITIAL_SENSOR_THRESHOLD = 1000;
        public float Threshold { get; set; }

        private readonly Timer _timer;
        private readonly MqttService _mqttService;

        public SensorService(MqttService mqttService)
        {
            _mqttService = mqttService;

            Threshold = INITIAL_SENSOR_THRESHOLD;
            _timer = new Timer(Threshold);
            _timer.Elapsed += OnTimerEvent;
            _timer.Start();
        }

        private async void OnTimerEvent(object sender, ElapsedEventArgs args)
        {
            if (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }

            if (_mqttService.IsConnected())
                await _mqttService.Publish("New data", "sensor/data");
        }
    }
}
