using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Assets
{
    public class TargetService : ITargetService
    {
        private readonly string _uri = "https://admin-api.arsounds.local/api/targets/vws";
        private readonly AuthenticationHeaderValue _authenticationHeaderValue;

        public TargetService(AuthenticationHeaderValue authenticationHeaderValue)
        {
            _authenticationHeaderValue = authenticationHeaderValue;
        }

        public async Task<ResponseMessage<TargetModel>> Get(string id)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var url = $"{_uri}/{id}";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = _authenticationHeaderValue;
            var response = await httpClient.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseMessage<TargetModel>>(result);
        }
    }
}