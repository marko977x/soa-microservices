using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DataMicroservice.Services
{
    public class MqttService
    {
        public const string PUBLIC_MQTT_SERVER_SOCKET = "broker.hivemq.com:8000/mqtt";
        private IMqttClient _client;

        public MqttService()
        {
            _client = (new MqttFactory()).CreateMqttClient();
        }

        public async Task Connect()
        {
            await _client.ConnectAsync((new MqttClientOptionsBuilder()).WithWebSocketServer(PUBLIC_MQTT_SERVER_SOCKET).Build(), CancellationToken.None);
        }

        public async Task Publish(object data, string topic)
        {
            MqttApplicationMessage message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonConvert.SerializeObject(data))
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _client.PublishAsync(message, CancellationToken.None);
        }

        public async Task Subscribe(string topic)
        {
            await _client.SubscribeAsync(topic);
        }
    }
}
