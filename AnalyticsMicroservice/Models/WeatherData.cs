using System.Reflection;

namespace AnalyticsMicroservice.Models
{
    public class WeatherData
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }

        public WeatherData()
        {
            Temperature = null;
            Humidity = null;
            Pressure = null;
        }

        public bool Check()
        {
            if (Temperature != null || Humidity != null || Pressure != null)
                return true;
            return false;
        }

        public void Clear()
        {
            Temperature = null;
            Humidity = null;
            Pressure = null;
        }

        public object this[string key]
        {
            get
            {
                PropertyInfo info = this.GetType().GetProperty(key);
                if (info == null)
                    return null;
                return info.GetValue(this, null);
            }
            set
            {
                PropertyInfo info = this.GetType().GetProperty(key);
                if (info != null)
                    info.SetValue(this, value, null);
            }
        }
    }
}