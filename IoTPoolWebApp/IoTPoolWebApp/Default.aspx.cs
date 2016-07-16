using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Threading.Tasks;
using IoTPoolWebApp.Models;
using Newtonsoft.Json;
using System.Threading;


namespace IoTPoolWebApp
{
    public partial class _Default : Page
    {
        private static string apiKey = ""; //Add API key here, case sensitive
        private IoTPool pool;
        protected void Page_Load(object sender, EventArgs e)
        {
            pool = new IoTPool();
            GetCurrentPoolData().Wait();
        }

        protected void btnPoolOn_Click(object sender, EventArgs e)
        {
            TurnPoolOn(true);
            Thread.Sleep(2000); //Wait 2 sec for Gateway to send D2C message to IoT Hub, ASA and SQL before querying new status
            GetCurrentPoolData().Wait();
        }

        protected void btnPoolOff_Click(object sender, EventArgs e)
        {
            TurnPoolOn(false);
            Thread.Sleep(2000); //Wait 2 sec for Gateway to send D2C message to IoT Hub, ASA and SQL before querying new status
            GetCurrentPoolData().Wait();
        }

        private async Task<HttpResponseMessage> TurnPoolOn(bool value)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage;

            if (!value)
            {
                // Add a new Request Message
                requestMessage = new HttpRequestMessage(HttpMethod.Put, "http://<yourwebsite>.azurewebsites.net/api/pool/poweroff");
            }
            else
            {
                // Add a new Request Message
                requestMessage = new HttpRequestMessage(HttpMethod.Put, "http://<yourwebsite>.azurewebsites.net/api/pool/poweron");
            }
            // Add our custom headers
            requestMessage.Headers.Add("api-key", apiKey);

            // Send the request to the server
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

            return response;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GetCurrentPoolData().Wait();
        }

        private async Task GetCurrentPoolData()
        {

            HttpClient httpClient = new HttpClient();
            try
            {
                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://<yourwebsite>.azurewebsites.net/api/pool/GetLatest");

                // Add our custom headers
                requestMessage.Headers.Add("api-key", apiKey);

                HttpResponseMessage response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                dynamic data = null;
                if (response != null)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject(json);
                }
                pool.PoolName = (string)data["PoolName"];
                pool.PoolWaterTempC = (double)data["PoolWaterTempC"];
                pool.OutsideAirTempC = (double)data["OutsideAirTempC"];
                pool.IsPoolPowerOn = (int)data["IsPoolPowerOn"];
                pool.SampleDateTime = (DateTime)data["SampleDateTime"];
            }
            catch (Exception e)
            {
                lblLastUpdated.Text = e.Message;
            }

            lblLastUpdated.Text = pool.SampleDateTime.ToString();
            lblOutsideAirTemp.Text = pool.OutsideAirTempC.ToString();
            lblPoolWaterTemp.Text = pool.PoolWaterTempC.ToString();
            if (pool.IsPoolPowerOn == 1)
            {
                lblPoolPower.Text = "On";
            }
            else
            {
                lblPoolPower.Text = "Off";
            }

        }
    }
}