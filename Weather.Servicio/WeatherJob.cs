using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;
using EnvioAlertas.Servicio;
using System.Net.Http.Formatting;

namespace Weather.Servicio
{
    public  class WeatherJob: IJob
    {
        static HttpClient client = new HttpClient();
        private const string URL = "http://api.openweathermap.org/data/2.5/weather";
        

        public void Execute(IJobExecutionContext context)
        {
            string urlParameters = string.Format(@"?q={0}&appid={1}", ConfigurationManager.AppSettings["City"].ToString(), ConfigurationManager.AppSettings["APIKey"].ToString());

            try
            {
                JobKey key = context.JobDetail.Key;

                Logger.WriteEntry("Starting Job Weather");

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    
                    var dataobject = response.Content.ReadAsAsync<Root>().Result;
                    var newLine = $"{dataobject.main.temp},F,{ (dataobject.weather[0].main=="Rain"?true:false).ToString()}";
                    File.AppendAllText(ConfigurationManager.AppSettings["filePath"].ToString(), newLine);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }catch(Exception ex)
            {
                Logger.WriteEntry(ex.ToString());
            }

        }



    }
}
