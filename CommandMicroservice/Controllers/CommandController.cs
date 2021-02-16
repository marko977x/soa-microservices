using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommandMicroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommandController : ControllerBase
    {

        [HttpPost]
        public void PostCommand([Required, FromBody]string command)
        {
            Console.WriteLine(command);
            if (command.ToLower().Contains("temperature"))
            {
                ShutDownSensor("temperature");
                return;
            }
            if (command.ToLower().Contains("pressure"))
            {

                ShutDownSensor("pressure");
                return;
            }
            if (command.ToLower().Contains("humidity"))
            {
                ShutDownSensor("humidity");
                return;
            }
        }

        private async void ShutDownSensor(string sensorType)
        {
            Console.WriteLine(sensorType);
            HttpClient httpClient = new HttpClient();
            try
            {
                var responseMessage = await httpClient.PostAsync("http://localhost:5000/api/Device/TurnOnOffSensor/" + 
                    sensorType, new StringContent("false", Encoding.UTF8, "application/json"));
                Console.WriteLine($"post response: {responseMessage}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
