namespace AnalyticsMicroservice.Models
{
    public class SensorData
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }

        public bool Check()
        {
            if (Temperature == null || Humidity == null || Pressure == null)
                return false;
            return true;
        }
    }
}