using DeviceMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMicroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly SensorService _sensorService;
        public DeviceController(SensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet]
        public ActionResult GetSensorParameters()
        {
            return Ok(_sensorService.Threshold);
        }

        [HttpPost]
        public ActionResult SetSensorParameters(float threshold)
        {
            _sensorService.Threshold = threshold;
            return Ok();
        }

        [HttpPost]
        public ActionResult SetSensorCommand(string command)
        {
            return Ok();
        }
    }
}
