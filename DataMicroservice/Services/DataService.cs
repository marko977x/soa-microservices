using MQTTnet;
using System;
using System.Text;

namespace DataMicroservice.Services
{
    public class DataService
    {
        private MqttService _mqttService;
        private IInfluxDBService _database;

        private event EventHandler ServiceCreated;
        public DataService(MqttService mqttService, IInfluxDBService database)
        {
            _mqttService = mqttService;
            _database = database;

            ServiceCreated += OnServiceCreated;
            ServiceCreated?.Invoke(this, EventArgs.Empty);
        }

        private async void OnServiceCreated(object sender, EventArgs args)
        {
            if (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }

            await _mqttService.Subscribe("sensor/data", OnDataReceived);
        }

        private async void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            Console.WriteLine(Encoding.UTF8.GetString(arg.ApplicationMessage.Payload));
            _database.Write("mem,host=host1 used_percent=23.43234543");
            await _mqttService.Publish(Encoding.UTF8.GetString(arg.ApplicationMessage.Payload), "data-analytics/data");
        }
    }
}
