using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace MobileApp
{
    class DataService
    {
        private static string apiKey = ""; //Case sensitive API key
        public static async Task<dynamic> getDataFromService(string queryString)
        {
            HttpClient httpClient = new HttpClient();

            // Add a new Request Message
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, queryString);

            // Add our custom headers
            requestMessage.Headers.Add("api-key", apiKey);

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }
    }
}
