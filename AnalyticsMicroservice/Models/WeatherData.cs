using System.Reflection;
using AnalyticsMicroservice.Services;


namespace AnalyticsMicroservice.Models
{
    public class WeatherData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }

        public WeatherData()
        {
            Temperature = null;
            Humidity = null;
            Pressure = null;
        }

        public bool Check()
        {
            if (Temperature != null && Humidity != null && Pressure != null)
                return true;
            return false;
        }

        public void Clear()
        {
            Temperature = null;
            Humidity = null;
            Pressure = null;
        }

        private double UncomfortIndex()
        {
            double humidity = Humidity;
            double temperature = Temperature;
            return (temperature - 0.55 * (1 - humidity * (temperature - 14.5)));
        }

        public string Analyze()
        {
            string result = null;
            const double rainPressure = 1000.0;
            const double sunnyPressure = 1020;
            const double rainHumidity = 0.9;
            const double snowTemperatureHigh = 2.0;
            const double snowTemperatureLow = -2.0;
            const double temperatureBroken = 50.0;
            const double humidityBrokenHigh = 1.0; 
            const double humidityBrokenLow = 0.0;
            const double pressureBrokenHigh = 1100.0;
            const double pressureBrokenLow = 800.0;

            double pressure = Pressure;
            double humidity = Humidity;
            double temperature = Temperature;
            double uncomfortIndex = UncomfortIndex();
            string[] analyticsEvents = AnalyticsService.Events;
            if (pressure > sunnyPressure)
            {
                if (uncomfortIndex <= 21)
                {
                    return AnalyticsService.Events[0];
                }
                else if (uncomfortIndex > 21 && uncomfortIndex < 24)
                {
                    return AnalyticsService.Events[1];
                }
                else return AnalyticsService.Events[2];
            }
            if (pressure < rainPressure && humidity > rainHumidity)
            {
                if (temperature >= -snowTemperatureLow && temperature <= snowTemperatureHigh)
                {
                    return AnalyticsService.Events[3];
                }
                if (temperature > snowTemperatureHigh)
                    return AnalyticsService.Events[4];
            }
            if (pressure < rainPressure && humidity < rainHumidity)
            {
                return AnalyticsService.Events[5];
            }
            if (pressure > rainPressure && pressure < sunnyPressure && humidity > rainHumidity)
                return AnalyticsService.Events[6];
            else if (pressure > rainPressure && pressure < sunnyPressure && humidity < rainHumidity)
                return AnalyticsService.Events[7];
            else if (temperature > temperatureBroken || temperature < -temperatureBroken)
                return AnalyticsService.Events[8];
            else if (humidity > humidityBrokenHigh || humidity < humidityBrokenLow)
                return AnalyticsService.Events[9];
            else if (pressure > pressureBrokenHigh || pressure < pressureBrokenLow)
                return AnalyticsService.Events[10];

            return result;
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