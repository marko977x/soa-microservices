using MQTTnet;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DataMicroservice.Services
{
    public class DataService
    {
        private MqttService _mqttService;

        private event EventHandler ServiceCreated;
        public DataService(MqttService mqttService)
        {
            _mqttService = mqttService;
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

        private void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            Console.WriteLine(Encoding.UTF8.GetString(arg.ApplicationMessage.Payload));
        }
    }
}
