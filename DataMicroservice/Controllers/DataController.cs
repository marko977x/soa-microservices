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
    [Route("api/[controller]/[action]")]
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


        [HttpGet("{sensorType}")]
        async public Task<IActionResult> GetSensorCurrentValue([Required, FromRoute] string sensorType)
        {

            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -2m) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> filter(fn: (r) => r.sensor == \"{sensorType.ToLower()}\")" +
                $"|> last()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            var fluxRecord = fluxTables[0].Records[0];
            return Ok(fluxRecord);
        }

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsCurrentValues()
        {

            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -2m) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> last()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            List<FluxRecord> fluxRecords = new List<FluxRecord>();
            fluxTables.ForEach(fluxTable =>
            {
                fluxTable.Records.ForEach(fluxRecord =>
                {
                    fluxRecords.Add(fluxRecord);
                });
            });
            return Ok(fluxRecords);
        }

        [HttpGet("{sensorType}")]
        async public Task<IActionResult> GetMaxValue([Required, FromRoute] string sensorType)
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: 2021-01-01T00:00:00.0Z, stop: now()) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> filter(fn: (r) => r.sensor == \"{sensorType.ToLower()}\")" +
                $"|> max()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            var fluxRecord = fluxTables[0].Records[0];
            return Ok(fluxRecord);
        }

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsMaxValues()
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: 2021-01-01T00:00:00.0Z, stop: now()) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> max()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            List<FluxRecord> fluxRecords = new List<FluxRecord>();
            fluxTables.ForEach(fluxTable =>
            {
                fluxTable.Records.ForEach(fluxRecord =>
                {
                    fluxRecords.Add(fluxRecord);
                });
            });
            return Ok(fluxRecords);
        }

        [HttpGet("{sensorType}")]
        async public Task<IActionResult> GetMinValue([Required, FromRoute] string sensorType)
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: 2021-01-01T00:00:00.0Z, stop: now()) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> filter(fn: (r) => r.sensor == \"{sensorType.ToLower()}\")" +
                $"|> min()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            var fluxRecord = fluxTables[0].Records[0];
            return Ok(fluxRecord);
        }

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsMinValues()
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: 2021-01-01T00:00:00.0Z, stop: now()) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> min()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            List<FluxRecord> fluxRecords = new List<FluxRecord>();
            fluxTables.ForEach(fluxTable =>
            {
                fluxTable.Records.ForEach(fluxRecord =>
                {
                    fluxRecords.Add(fluxRecord);
                });
            });
            return Ok(fluxRecords);
        }

        [HttpGet("{sensorType}")]
        async public Task<IActionResult> GetLastNHoursMeanValue([Required, FromRoute] string sensorType, [Required, FromQuery(Name ="hours")] int hours)
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -{hours}h) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> filter(fn: (r) => r.sensor == \"{sensorType.ToLower()}\")" +
                $"|> mean()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            var fluxRecord = fluxTables[0].Records[0];
            return Ok(fluxRecord);
        }

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsLastNHoursMeanValues([Required, FromQuery(Name = "hours")] int hours)
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -{hours}h) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\") " +
                $"|> mean()";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            List<FluxRecord> fluxRecords = new List<FluxRecord>();
            fluxTables.ForEach(fluxTable =>
            {
                fluxTable.Records.ForEach(fluxRecord =>
                {
                    fluxRecords.Add(fluxRecord);
                });
            });
            return Ok(fluxRecords);
        }

        [HttpGet("{sensorType}")]
        async public Task<IActionResult> GetLastNMinutesValues([Required, FromRoute]string sensorType, [Required, FromQuery(Name = "minutes")] int minutes)
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -{minutes}m) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r.sensor == \"{sensorType.ToLower()}\")" +
                $"|> filter(fn: (r) => r._field == \"value\")";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            List<FluxRecord> fluxRecords = new List<FluxRecord>();
            fluxTables.ForEach(fluxTable =>
            {
                fluxTable.Records.ForEach(fluxRecord =>
                {
                    fluxRecords.Add(fluxRecord);
                });
            });
            return Ok(fluxRecords);
        }

        [HttpGet]
        async public Task<IActionResult> GetAllSensorsLastNMinutesValues([Required, FromQuery(Name ="minutes")] int minutes)
        {
            string query = $"from(bucket: \"soa\") " +
                $"|> range(start: -{minutes}m) " +
                $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
                $"|> filter(fn: (r) => r._field == \"value\")";
            List<FluxTable> fluxTables = await _influxDBService.Query(query);
            List<List<FluxRecord>> fluxRecords = new List<List<FluxRecord>>();
            fluxTables.ForEach(fluxTable =>
            {
                fluxRecords.Add(fluxTable.Records);
            });
            return Ok(fluxRecords);
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
