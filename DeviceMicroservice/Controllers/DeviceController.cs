using DeviceMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DeviceMicroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly SensorsListService _sensorsListService;
        public DeviceController(SensorsListService sensorService)
        {
            _sensorsListService = sensorService;
        }

        [HttpGet("{type}")]
        public IActionResult GetSensorParams([Required, FromRoute] string type)
        {
            if (type == null)
                return BadRequest($"Sensor type required");

            foreach (SensorService sensor in this._sensorsListService.SensorsList)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };
                    return Ok(JsonSerializer.Serialize(sensor, options));
                }
            }
            return BadRequest($"Sensor type: {type} doesn't exist!");
        }

        [HttpGet]
        public IActionResult GetAllSensorsParams()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(this._sensorsListService.SensorsList, options));
        }

        [HttpGet("{type}")]
        public IActionResult GetTimeout([Required, FromRoute] string type)
        {
            foreach (var sensor in _sensorsListService.SensorsList)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };

                    string timeoutInfo = JsonSerializer.Serialize(new
                    {
                        isTimeout = !sensor.IsTreshold,
                        value = sensor.Timeout
                    }, options);

                    return Ok(timeoutInfo);
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpGet("{type}")]
        public IActionResult GetThreshold([Required, FromRoute] string type)
        {
            foreach (var sensor in _sensorsListService.SensorsList)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };
                    string tresholdInfo = JsonSerializer.Serialize(new { isTreshold = sensor.IsTreshold, value = sensor.Threshold }, options);
                    return Ok(tresholdInfo);
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpPost("{type}")]
        public IActionResult TurnOnOffSensor(
           [Required, FromBody] bool on, [Required, FromRoute] string type)
        {
            foreach (SensorService sensor in _sensorsListService.SensorsList)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    if (on)
                    {
                        if (!sensor.IsMeasuring)
                        {
                            sensor.StartSensor();
                            return Ok($"Sensor {type} turned on");
                        }
                        return Ok($"Sensor {type} alredy stopped");
                    }
                    else
                    {
                        if (sensor.IsMeasuring)
                        {
                            sensor.StopSensor();
                            return Ok($"Sensor {type} turned off");
                        }
                        return Ok($"Sensor {type} alredy stopped");
                    }
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpPost("{type}")]
        public IActionResult SetTimeout(
            [Required, FromRoute] string type, double? value)

        {
            System.Console.WriteLine(value);
            foreach (var sensor in this._sensorsListService.SensorsList)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    sensor.IsTreshold = false;
                    if (value != null)
                    {
                        sensor.SetTimeout((double)value);
                        return Ok($"Timeout based measuring started for {type} sensor. New Timeout value set");
                    }
                    else
                    {
                        return Ok($"Timeout based measuring started for {type} sensor. Last Timeout value used");
                    }
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }


        [HttpPost("{type}")]
        public IActionResult SetThreshold(
            [Required, FromRoute] string type, [FromBody] double? value)
        {
            if (value == null) return BadRequest("Provide treshold value");

            foreach (var sensor in _sensorsListService.SensorsList)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    sensor.IsTreshold = true;
                    if (value != null)
                    {
                        sensor.Threshold = (float)value;
                        return Ok($"Treshold based measuring started for {type} sensor. New Treshold value set");
                    }
                    else
                    {
                        return Ok($"Treshold based measuring started for {type} sensor. Default Treshold value used");
                    }
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }
    }
}
