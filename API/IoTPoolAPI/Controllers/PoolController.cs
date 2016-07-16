using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IoTPoolAPI.Models;
using Microsoft.Azure.Devices;
using System.Text;

namespace IoTPoolAPI.Controllers
{
    public class PoolController : ApiController
    {
        private IoTPoolAPIContext db = new IoTPoolAPIContext();
        private string connectionString = "HostName=<iothubname>.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=<key>";
        private string deviceNameInIoTHub = "iotpool";
        private string apiKey = "<set API key here>"; //Case sensitive API key


        // GET: api/Pool
        // Return pool telemetry data from last 48 hours
        [HttpGet]
        public IQueryable<PoolDTO> GetLastTwoDays()
        {
            IQueryable<PoolDTO> pooldatatwodays = null;
            bool isAuthorized = IsValidAPIKey();
            if (isAuthorized)
            {
                DateTime today = DateTime.UtcNow;
                DateTime twodaysago = today.AddDays(-2); //Only fetch last 2 days of data

                pooldatatwodays = from p in db.Pools
                                      where p.SampleDateTime > twodaysago
                                      select new PoolDTO()
                                      {
                                          Id = p.Id,
                                          PoolName = p.PoolName,
                                          PoolWaterTempC = p.PoolWaterTempC,
                                          OutsideAirTempC = p.OutsideAirTempC,
                                          IsPoolPowerOn = p.IsPoolPowerOn,
                                          SampleDateTime = p.SampleDateTime
                                      };
            }

            return pooldatatwodays;
        }

        // GET: api/Pool/GetLatest
        // Return lastest pool telemetry data
        [HttpGet]
        public Pool GetLatest()
        {
            Pool latestpooldata = null;
            bool isAuthorized = IsValidAPIKey();
            if (isAuthorized)
            {
                var pooldata = from p in db.Pools
                               select p;
                latestpooldata = pooldata.OrderByDescending(message => message.Id).FirstOrDefault();
            }

            return latestpooldata;

        }

        // PUT: api/Pool/PowerOn
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PowerOn()
        {
            bool isAuthorized = IsValidAPIKey();
            if (isAuthorized)
            {
                try
                {
                    ServiceClient serviceClient;
                    serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
                    var commandMessage = new Message(Encoding.ASCII.GetBytes("1")); //1 indicates power on
                    await serviceClient.SendAsync(deviceNameInIoTHub, commandMessage);
                }
                catch
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }
                return StatusCode(HttpStatusCode.OK);
            }


            return StatusCode(HttpStatusCode.Forbidden);
        }

        // PUT: api/Pool/PowerOff
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PowerOff()
        {
            bool isAuthorized = IsValidAPIKey();
            if (isAuthorized)
            {
                try
                {
                    ServiceClient serviceClient;
                    serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
                    var commandMessage = new Message(Encoding.ASCII.GetBytes("0")); //0 indicates power off
                    await serviceClient.SendAsync(deviceNameInIoTHub, commandMessage);
                }
                catch
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }
                return StatusCode(HttpStatusCode.OK);
            }

            return StatusCode(HttpStatusCode.Forbidden);

        }

        private bool IsValidAPIKey()
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string key = string.Empty;
            if (headers.Contains("api-key"))
            {
                key = headers.GetValues("api-key").First();
            }
            if (key == apiKey)
            {
                return true;
            }

            return false;
        }
    }
}