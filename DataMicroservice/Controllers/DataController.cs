using DataMicroservice.DataModels;
using DataMicroservice.Services;
using InfluxDB.Client.Core.Flux.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IInfluxDBService _influxDBService;
        private readonly DataService _dataService;

        public DataController(IInfluxDBService influxDBService, DataService dataService)
        {
            this._influxDBService = influxDBService;
            this._dataService = dataService;
        }

        // GET: api/<DataController>
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //}

        // GET api/<DataController>/5
        [HttpGet("{sensorType}")]
        [Route("getsensordata")]
        async public Task<IActionResult> GetSensorData([FromQuery] string sensorType)
        {
            Console.WriteLine($"sensorType: {sensorType}");
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -50m) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> filter(fn: (r) => r.sensor == \"{sensorType}\")";
            Console.WriteLine(query);
            List<FluxTable> query_data = await _influxDBService.Query(query);
            Console.WriteLine("query data:");
            foreach (var data_point in query_data)
            {
                Console.WriteLine(data_point);
            }
            return Ok(query_data);
        }

        [HttpGet("{minutes}")]
        [Route("getlastnminutesdata")]
        async public Task<IActionResult> GetLastNMinutesData([FromQuery] int minutes)
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -{minutes}m) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") ";
            List<FluxTable> query_data = await _influxDBService.Query(query);
            return Ok(query_data);
        }

        // POST api/<DataController>
        [HttpPost]
        public void Post([FromBody, Required] SensorData data)
        {
            this._dataService.saveData(data);
        }

        // PUT api/<DataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
