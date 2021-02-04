using AnalyticsMicroservice.Models;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace AnalyticsMicroservice.Services
{
    public class AnalyticsService
    {
        public static readonly string[] Events = { "EVENT1", "EVENT2", "EVENT3" };
        private MqttService _mqttService;
        private IInfluxDBService _database;
        private event EventHandler ServiceCreated;
        private WeatherData _model;
        public AnalyticsService(MqttService mqttService, IInfluxDBService database)
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

            await _mqttService.Subscribe("data-analytics/data", OnDataReceived);
        }

        private void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            SensorData data = JsonConvert.DeserializeObject<SensorData>(
                Encoding.UTF8.GetString(arg.ApplicationMessage.Payload));

            _model[data.SensorType] = data.Value;

            if (!_model.Check()) return;

            if (Events.Contains(GetEventBasedOnModel()))
            {
                //_database.Write(Encoding.UTF8.GetString(arg.ApplicationMessage.Payload));
                //SendActionRequestToCommandMicroservice("Command");
            }

            _model.Clear();
        }

        private string GetEventBasedOnModel()
        {
            // analyze this._model
            return "";
        }

        private async void SendActionRequestToCommandMicroservice(string command)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                await httpClient.PostAsync("", new StringContent(command));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}