using DataMicroservice.DataModels;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using MQTTnet;
using Newtonsoft.Json;
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
            var json_data = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            //Console.WriteLine(json_data);
            SensorData sensorData = JsonConvert.DeserializeObject<SensorData>(json_data);
            this.saveData(sensorData);
            await _mqttService.Publish(Encoding.UTF8.GetString(arg.ApplicationMessage.Payload), "data-analytics/data");
        }

        public void saveData(SensorData sensorData)
        {
            var point = PointData
                      .Measurement("SensorsData")
                      .Tag("sensor", sensorData.SensorType.ToLower())
                      .Field("value", sensorData.Value)
                      .Timestamp(DateTime.UtcNow, WritePrecision.Ms);
            _database.Write(point);
            //Console.WriteLine("added to db");
        }
    }
}
