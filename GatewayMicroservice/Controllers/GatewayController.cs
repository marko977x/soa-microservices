using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GatewayMicroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GatewayController : Controller
    {
        private HttpClient _httpClient;

        public GatewayController()
        {
            this._httpClient = new HttpClient();
        }


        private async Task<ContentResult> ProxyPost(string url, string jsonBody)
        {
            var responseMessage = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            return Content(await responseMessage.Content.ReadAsStringAsync());
        }
        private async Task<ContentResult> ProxyGet(string url)
            => Content(await _httpClient.GetStringAsync(url));

        [HttpGet]
        public async Task<IActionResult> GetCommandList()
            => await ProxyGet("http://localhost:5000/api/Device/GetCommandList/");

        [HttpGet("{type}")]
        public async Task<IActionResult> GetSensorParams([Required, FromRoute] string type)
            => await ProxyGet("http://localhost:5000/api/Device/GetSensorParams/" + type);


        [HttpGet]
        public async Task<IActionResult> GetAllSensorsParams()
            => await ProxyGet("http://localhost:5000/api/Device/GetAllSensorsParams/");

        [HttpGet("{type}")]
        public async Task<IActionResult> GetTimeout([Required, FromRoute] string type)
            => await ProxyGet("http://localhost:5000/api/Device/GetTimeout/" + type);

        [HttpGet("{type}")]
        public async Task<IActionResult> GetThreshold([Required, FromRoute] string type)
            => await ProxyGet("http://localhost:5000/api/Device/GetThreshold/" + type);

        [HttpPost("{type}")]
        public async Task<IActionResult> TurnOnOffSensor([Required, FromRoute] string type, [Required, FromBody] bool on)
            => await ProxyPost("http://localhost:5000/api/Device/TurnOnOffSensor/" + type, JsonSerializer.Serialize(on, typeof(bool)));

        [HttpPost("{type}")]
        public async Task<IActionResult> SetTimeout(
            [Required, FromRoute] string type, [FromBody] double? value)
            => await ProxyPost("http://localhost:5000/api/Device/SetTimeout/" + type, JsonSerializer.Serialize(value, typeof(double)));

        [HttpPost("{type}")]
        public async Task<IActionResult> SetThreshold(
            [Required, FromRoute] string type, [Required, FromBody] double? value)
            => await ProxyPost("http://localhost:5000/api/Device/SetThreshold/" + type, JsonSerializer.Serialize(value, typeof(double)));

        //DATA ENDPOINTS
        [HttpGet("{type}")]
        async public Task<IActionResult> GetSensorCurrentValue([Required, FromRoute] string type)
            => await ProxyGet("http://localhost:5002/api/Data/GetSensorCurrentValue/" + type);

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsCurrentValues()
            => await ProxyGet("http://localhost:5002/api/Data/GetAllSensorsCurrentValues");

        [HttpGet("{type}")]
        async public Task<IActionResult> GetMaxValue([Required, FromRoute] string type)
            => await ProxyGet("http://localhost:5002/api/Data/GetMaxValue/" + type);

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsMaxValues()
            => await ProxyGet("http://localhost:5002/api/Data/GetAllSensorsMaxValues");

        [HttpGet("{type}")]
        async public Task<IActionResult> GetMinValue([Required, FromRoute] string type)
            => await ProxyGet("http://localhost:5002/api/Data/GetMinValue/" + type);

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsMinValues()
            => await ProxyGet("http://localhost:5002/api/Data/GetAllSensorsMinValues");

        [HttpGet("{type}")]
        async public Task<IActionResult> GetLastNHoursMeanValue([Required, FromRoute] string type, [Required, FromQuery(Name = "hours")] int? hours)
            => await ProxyGet("http://localhost:5002/api/Data/GetLastNHoursMeanValue/" + type + "?hours=" + hours.ToString());

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsLastNHoursMeanValues([Required, FromQuery(Name = "hours")] int? hours)
            => await ProxyGet("http://localhost:5002/api/Data/GetAllSensorsLastNHoursMeanValues" + "?hours=" + hours.ToString());

        [HttpGet("{type}")]
        async public Task<IActionResult> GetLastNMinutesValues([Required, FromRoute] string type, [Required, FromQuery(Name = "minutes")] int? minutes)
            => await ProxyGet("http://localhost:5002/api/Data/GetLastNMinutesValues/" + type + "?minutes=" + minutes.ToString());

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsLastNMinutesValues([Required, FromQuery(Name = "minutes")] int? minutes)
            => await ProxyGet("http://localhost:5002/api/Data/GetAllSensorsLastNMinutesValues" + "?minutes=" + minutes.ToString());

        //private async Task<string> SendGetRequest(string url)
        //{
        //    string data;
        //    HttpClient httpClient = new HttpClient();
        //    try
        //    {
        //        var responseMessage = await httpClient.GetAsync(url);
        //        using (HttpContent content = responseMessage.Content)
        //        {
        //            data = content.ToString();
        //        }
        //        Console.WriteLine($"get response: {data}");
        //        return data;
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //        return null;
        //    }
        //}

    }
}
