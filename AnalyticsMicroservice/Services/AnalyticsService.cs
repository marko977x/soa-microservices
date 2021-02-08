using AnalyticsMicroservice.Models;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
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
        public static readonly string[] Events = { "Vedro, pogodno za izlazak", "Vedro, nepogodno za hronicne bolesnike",
            "Vedro, ne preporucuje se izlazak napolje", "Sneg", "Kisa", "Oblacno", "Magla", "Relativno oblacno", "Temperature senzor pokvaren",
            "Humidity senzor pokvaren", "Pressure senzor pokvaren"};
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
            string eventVal = GetEventBasedOnModel();
            if (Events.Contains(eventVal))
            {
                var point = PointData
                      .Measurement("AnalyticsData")
                      .Field("event", eventVal)
                      .Timestamp(DateTime.UtcNow, WritePrecision.Ms);
                _database.Write(point);
                SendActionRequestToCommandMicroservice(eventVal);
                SendEventToWebDashboard(eventVal);
            }
            _model.Clear();
        }

        private string GetEventBasedOnModel()
        {
            // analyze this._model
            return _model.Analyze();
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

        private async void SendEventToWebDashboard(string eventVal)
        {

        }
    }
}