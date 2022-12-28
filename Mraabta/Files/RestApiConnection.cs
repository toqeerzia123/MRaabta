using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using System.Text;
using MRaabta.Files;
using static MRaabta.Files.TrackingModel;

namespace MRaabta.Files
{
    internal class RestApiConnection
    {
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        //private const string URL = "http://172.16.0.14/CODAPIAMIR/api/";
        private const string URL = "http://172.16.0.14/CODAPIAMIRLIVE/api/";
        // private const string URL = "http://localhost:39209//api/";
        // private const string URL = "http://mnpcourier.com/mycodapi//api/";

        public class DataObject
        {
            public string Name { get; set; }
        }
        public RestApiConnection()
        {

        }
        public IEnumerable<result_batch_updated> getAPI_trackingAsync(string URL)
        {
            IEnumerable<result_batch_updated> result_tracking = null;
            using (var client = new HttpClient())
            {
                try
                {
                    //client.BaseAddress = new Uri("http://20.46.47.21/MPELAPI/api/");
                    //client.BaseAddress = new Uri("http://localhost:39209//api/");
                    //client.BaseAddress = new Uri("http://172.16.0.14/CODAPIAMIR//api/");
                    client.BaseAddress = new Uri("http://mnpcourier.com/mycodapi//api/");

                    //Called Member default GET All records  
                    //GetAsync to send a GET request   
                    // PutAsync to send a PUT request  
                    //var responseTask = client.GetAsync("Tracking/Consignment_Tracking?Username=test_user&password=12345&consignment=544794010000096");
                    var responseTask = client.GetAsync(URL);
                    responseTask.Wait();

                    //To store result of web api response.   
                    var result = responseTask.Result;

                    //If success received   
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<result_batch_updated>>();
                        readTask.Wait();

                        result_tracking = readTask.Result;
                    }

                    else
                    {
                        //Error response received   
                        result_tracking = null;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return result_tracking;
            }
        }

    }
}