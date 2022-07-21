using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace worker
{
    public class ServiceClient {

        public static async Task PostMessage(string postData)
        {
            var json = JsonConvert.SerializeObject(postData);
            var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    var result = await client.PostAsync("http://publisher_api:80/api/Values", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                }
            }
        }
    }
}