using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using ARSounds.ApiClient.Contracts;
using ARSounds.ApiClient.Data;
using ARSounds.ApiClient.Dtos;
using ARSounds.ApiClient.Response;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace ARSounds.ApiClient.Services;

public class TargetsService : ITargetsService
{
    #region Fields/Consts

    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    #endregion

    public TargetsService(IOptions<ARSoundsApiOptions> soundsApiOptions)
    {
        var options = soundsApiOptions.Value;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(options.Url)
        };
    }

    #region Methods

    public async Task<ResponseMessage<IEnumerable<TargetDto>>> GetAsync(string bearerToken, CancellationToken cancellationToken = default)
    {
        _httpClient.SetBearerToken(bearerToken);

        var response = await _httpClient.GetAsync("api/targets/?page=1&size=100", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to fetch targets: {response.StatusCode}");
        }

        var responseMessage = await response.Content.ReadFromJsonAsync<ResponseMessage<IEnumerable<TargetDto>>>(JsonSerializerOptions, cancellationToken);

        ArgumentNullException.ThrowIfNull(responseMessage, "Failed to deserialize response from targets API.");

        return responseMessage;
    }

    public async Task<ResponseMessage<TargetDto>> GetAsync(Guid id, string bearerToken, CancellationToken cancellationToken = default)
    {
        _httpClient.SetBearerToken(bearerToken);

        var response = await _httpClient.GetAsync($"api/targets/{id}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to fetch target {id}: {response.StatusCode}");
        }

        var responseMessage = await response.Content.ReadFromJsonAsync<ResponseMessage<TargetDto>>(JsonSerializerOptions, cancellationToken);

        ArgumentNullException.ThrowIfNull(responseMessage, "Failed to deserialize response from targets API.");

        return responseMessage;
    }

    #endregion
}
