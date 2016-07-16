using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp
{
    class Core
    {
        public static async Task<IoTPool> GetCurrentPoolData()
        {
            string queryString = "http://<yourwebsite>.azurewebsites.net/api/pool/GetLatest";
            dynamic results = await DataService.getDataFromService(queryString).ConfigureAwait(false);

            IoTPool pool = new IoTPool();
            pool.PoolName = (string)results["PoolName"];
            pool.PoolWaterTempC = (double)results["PoolWaterTempC"];
            pool.OutsideAirTempC = (double)results["OutsideAirTempC"];
            pool.IsPoolPowerOn = (int)results["IsPoolPowerOn"];
            pool.SampleDateTime = (DateTime)results["SampleDateTime"];

            return pool;
        }
    }
}
