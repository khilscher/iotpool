using System.ComponentModel.DataAnnotations;
using System;

namespace IoTPoolAPI.Models
{
    public class Pool
    {
        public int Id { get; set; }
        public string PoolName { get; set; }
        public double PoolWaterTempC { get; set; }
        public double OutsideAirTempC { get; set; }
        public int IsPoolPowerOn { get; set; }
        public DateTime SampleDateTime { get; set; }

    }

    // DTO http://www.asp.net/web-api/overview/data/using-web-api-with-entity-framework/part-5
    public class PoolDTO
    {
        public int Id { get; set; }
        public string PoolName { get; set; }
        public double PoolWaterTempC { get; set; }
        public double OutsideAirTempC { get; set; }
        public int IsPoolPowerOn { get; set; }
        public DateTime SampleDateTime { get; set; }

    }

}