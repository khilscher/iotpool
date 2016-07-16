using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;

namespace MobileApp
{
    public partial class PoolPage : ContentPage
    {
        private string apiKey = ""; //Case sensitive API key
        public PoolPage()
        {
            InitializeComponent();
            this.Title = "IoT Pool";
            getCurrentPoolDataBtn.Clicked += GetCurrentPoolDataBtn_Clicked;
            setPoolPowerOffBtn.Clicked += SetPoolPowerOffBtn_Clicked;
            setPoolPowerOnBtn.Clicked += SetPoolPowerOnBtn_Clicked;

            //Set the default binding to a default object for now
            this.BindingContext = new IoTPool();
        }

        private async void GetCurrentPoolDataBtn_Clicked(object sender, EventArgs e)
        {

            IoTPool pool = await Core.GetCurrentPoolData();
            this.BindingContext = pool;

        }

        private async void SetPoolPowerOnBtn_Clicked(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();

            // Add a new Request Message
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, "http://<yourwebsite>.azurewebsites.net/api/pool/poweron");

            // Add our custom headers
            requestMessage.Headers.Add("api-key", apiKey);

            // Send the request to the server
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        }

        private async void SetPoolPowerOffBtn_Clicked(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();

            // Add a new Request Message
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, "http://<yourwebsite>.azurewebsites.net/api/pool/poweroff");

            // Add our custom headers
            requestMessage.Headers.Add("api-key", apiKey);

            // Send the request to the server
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        }

    }

}
