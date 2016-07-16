using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp
{
    public class IoTPool
    {
        public int Id { get; set; }
        public string PoolName { get; set; }
        public double PoolWaterTempC { get; set; }
        public double OutsideAirTempC { get; set; }
        public int IsPoolPowerOn { get; set; }
        public DateTime SampleDateTime { get; set; }

        public IoTPool()
        {
            this.PoolName = "Refresh to get latest data";
            this.PoolWaterTempC = 0;
            this.OutsideAirTempC = 0;
            this.IsPoolPowerOn = 0;
            this.SampleDateTime = DateTime.UtcNow;
        }
    }
}
