using Microsoft.Extensions.DependencyInjection;

namespace DataMicroservice.Services
{
    public static class ServicesCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton(new MqttService());
        }
    }
}
