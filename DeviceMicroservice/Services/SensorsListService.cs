using System.Collections.Generic;

namespace DeviceMicroservice.Services
{
    public class SensorsListService
    {
        public List<SensorService> SensorsList { get; set; }

        public SensorsListService(MqttService mqttService)
        {
            this.SensorsList = new List<SensorService>
            {
                new SensorService(mqttService, "Temperature", "/app/deviceServices/weatherHistory.csv"),
                new SensorService(mqttService, "Humidity", "/app/deviceServices/weatherHistory.csv"),
                new SensorService(mqttService, "Pressure", "/app/deviceServices/weatherHistory.csv")
            };
        }

        public void AddSensor(SensorService newSensor)
        {
            this.SensorsList.Add(newSensor);
        }
    }
}
