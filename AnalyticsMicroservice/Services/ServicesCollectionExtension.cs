using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsMicroservice.Services
{
    public static class ServicesCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton(new MqttService());
            services.AddSingleton<IInfluxDBService, InfluxDBService>();
            services.AddSingleton<AnalyticsService>();
            services.AddTransient<IInfluxDBService, InfluxDBService>();
        }
    }
}
