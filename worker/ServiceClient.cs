using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace worker
{
    public interface IServiceClient
    {
        Task PostMessage(string postData);
    }

    public class ServiceClient : IServiceClient
    {
        private readonly IConfiguration _configuration;

        public ServiceClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task PostMessage(string postData)
        {
            var json = JsonConvert.SerializeObject(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using var client = new HttpClient(httpClientHandler);
            var url = _configuration.GetSection("API:Url").Value;
            var result = await client.PostAsync(url, content);
            string resultContent = await result.Content.ReadAsStringAsync();
        }
    }
}