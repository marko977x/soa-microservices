using Microsoft.Extensions.DependencyInjection;

namespace DeviceMicroservice.Services
{
    public static class ServicesCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton(new MqttService());
            services.AddSingleton(serviceProvider => new SensorsListService(serviceProvider.GetService<MqttService>()));
        }
    }
}
