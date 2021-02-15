using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMicroservice.Services
{
    public class InfluxDBService : IInfluxDBService
    {
        public const string DB_CONNECTION_URL = "http://influxdbData:8086";
        public const string DB_BUCKET = "soa";
        public const string DB_ORGANIZATION = "soa";
        public const string DB_USER = "admin";
        public readonly char[] DB_PASSWORD = "adminadmin".ToCharArray();

        private InfluxDBClient _client;

        public InfluxDBService()
        {
            CreateDatabase();

        }

        private void CreateDatabase()
        {
            _client = InfluxDBClientFactory.Create(DB_CONNECTION_URL, DB_USER, DB_PASSWORD);
        }

        public void Write(string data)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WriteRecord(DB_BUCKET, DB_ORGANIZATION, InfluxDB.Client.Api.Domain.WritePrecision.Ms, data);
            }
        }

        public void Write<T>(T data)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WriteMeasurement<T>(DB_BUCKET, DB_ORGANIZATION, InfluxDB.Client.Api.Domain.WritePrecision.Ms, data);
            }
        }

        public void Write(PointData point)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WritePoint(DB_BUCKET, DB_ORGANIZATION, point);
            }
        }

        public async Task<List<FluxTable>> Query(string query)
        {
            return await _client.GetQueryApi().QueryAsync(query, DB_ORGANIZATION);
        }
    }
}
