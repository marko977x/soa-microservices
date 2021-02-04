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
                new SensorService(mqttService, "Temperature", "weatherHistory.csv"),
                new SensorService(mqttService, "Humidity", "weatherHistory.csv"),
                new SensorService(mqttService, "Pressure", "weatherHistory.csv")
            };
        }

        public void AddSensor(SensorService newSensor)
        {
            this.SensorsList.Add(newSensor);
        }
    }
}
