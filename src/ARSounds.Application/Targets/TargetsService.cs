using ARSounds.Application.Auth;
using ARSounds.Application.ImageRecognition.Response;
using ARSounds.Core.Configuration;
using ARSounds.Core.Targets;
using Newtonsoft.Json;

namespace ARSounds.Application.ImageRecognition;

public class TargetsService : ITargetsService
{
    private readonly IAuthService _authService;
    private readonly AppConfiguration _appConfiguration;
    private readonly HttpClient _httpClient;

    public TargetsService(IAuthService authService, AppConfiguration appConfiguration)
    {
        _authService = authService;
        _appConfiguration = appConfiguration;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_appConfiguration.ApiUrl);
    }

    public async Task<ResponseMessage<IEnumerable<Target>>> GetAsync(CancellationToken cancellationToken = default)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.Token.AccessToken);

        var response = await _httpClient.GetAsync($"api/targets/?page=1&size=100", cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseMessage<IEnumerable<Target>>>(responseContent);
    }

    public async Task<ResponseMessage<Target>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.Token.AccessToken);

        var response = await _httpClient.GetAsync($"api/targets/{id}", cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseMessage<Target>>(responseContent);
    }
}
