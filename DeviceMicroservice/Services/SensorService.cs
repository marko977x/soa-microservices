using CsvHelper;
using CsvHelper.Configuration;
using DeviceMicroservice.DataModels;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace DeviceMicroservice.Services
{
    public class SensorService
    {
        public const float INITIAL_SENSOR_THRESHOLD = 1000;
        public float Threshold { get; set; }
        public double Timeout { get; set; }
        public bool IsThreshold { get; set; }
        public bool IsMeasuring { get; set; }
        public double Value { get; set; }
        public string SensorType { get; set; }
        public string _filePath;

        private readonly Timer _timer;
        private readonly MqttService _mqttService;
        private StreamReader _streamReader;
        private CsvReader _csv;

        public SensorService(MqttService mqttService, string sensorType, string filePath)
        {
            _mqttService = mqttService;

            Threshold = INITIAL_SENSOR_THRESHOLD;
            this.Timeout = 5000;
            _timer = new Timer(this.Timeout);
            _timer.Elapsed += OnTimerEvent;
            this.SensorType = sensorType;
            this._filePath = filePath;
            _timer.Start();
            this.IsMeasuring = true;
            this.setCsv();
            this.IsThreshold = false;
        }

        private void setCsv()
        {
            this._streamReader = new StreamReader(this._filePath);
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            this._csv = new CsvReader(_streamReader, config);
            _csv.Read();
            _csv.ReadHeader();
        }

        private async void OnTimerEvent(object sender, ElapsedEventArgs args)
        {
            if (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }

            if (_mqttService.IsConnected())
            {
                this.ReadValue();
                SensorData data = new SensorData(this.Value, this.SensorType);
                if (!this.IsThreshold)
                {
                    await _mqttService.Publish(data, "sensor/data");
                    Console.WriteLine(data.Value);
                }
                else if (data.Value > this.Threshold)
                {
                    await _mqttService.Publish(data, "sensor/data");
                    Console.WriteLine(data.Value);
                }
            }
        }

        public void SetTimeout(double interval)
        {
            _timer.Stop();
            Timeout = interval;
            _timer.Interval = interval;
            _timer.Start();
        }

        public void StopSensor()
        {
            IsMeasuring = false;
            _timer.Stop();
        }

        public void StartSensor()
        {
            IsMeasuring = true;
            _timer.Start();
        }
        private async Task SendValueAsync()
        {
            if (IsThreshold)
            {
                if (Value > Threshold)
                {

                }

            }
            else
            {

            }
        }

        private void ReadValue()
        {
            try
            {
                string sensor_value;
                if (_csv.Read())
                    sensor_value = _csv.GetField<string>(this.SensorType);
                else
                {
                    _streamReader.DiscardBufferedData();
                    using (this._csv) { }
                    this.setCsv();
                    _csv.Read();
                    sensor_value = _csv.GetField<string>(this.SensorType);
                }
                this.Value = double.Parse(sensor_value, CultureInfo.InvariantCulture);
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
