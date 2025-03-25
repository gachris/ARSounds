using ARSounds.Application.Auth;
using ARSounds.Application.ImageRecognition.Response;
using ARSounds.Core.Configuration;
using ARSounds.Core.Targets;
using Newtonsoft.Json;

namespace ARSounds.Application.ImageRecognition;

public class TargetsService : ITargetsService
{
    private readonly IAuthService? _authService;
    private readonly AppConfiguration _appConfiguration;
    private readonly HttpClient _httpClient;

    public TargetsService(AppConfiguration appConfiguration, IAuthService? authService = null)
    {
        _authService = authService;
        _appConfiguration = appConfiguration;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_appConfiguration.ApiUrl);
    }

    public async Task<ResponseMessage<IEnumerable<Target>>> GetAsync(CancellationToken cancellationToken = default)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService?.Token.AccessToken
            ?? "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQyRkE5RUExODYyMDJGNjMyRjc0MjZCQTk3QzNBQTc4IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE3NDI5NDY3NTksImV4cCI6MTc0Mjk1MDM1OSwiaXNzIjoiaHR0cHM6Ly9zdHMuc2tvcnViYS5sb2NhbCIsImF1ZCI6ImFyc291bmRzLmFwaSIsImNsaWVudF9pZCI6ImJhNzdhNTUwLWI5NDItNDc4ZC1hODg4LWExZDAyMzIxZjhkOCIsInN1YiI6ImRhYWQ3YjI3LWIxNDAtNGM1OC1hZGI5LWUwMDYzNDE5ZmI5ZCIsImF1dGhfdGltZSI6MTc0MjkzNTk5OSwiaWRwIjoibG9jYWwiLCJuYW1lIjoiYWRtaW4iLCJyb2xlIjoiU3VwZXJBZG1pbiIsImp0aSI6Ijc1MUJENDZBMDE4QjdDMzNDRDM2NDlGMzRCOUY3MzFBIiwic2lkIjoiOTkwQTBBNTVFMjYyRUFFQzNBQTZEN0YxQUQyQTQxMjUiLCJpYXQiOjE3NDI5NDY3NTksInNjb3BlIjpbIm9wZW5pZCIsImVtYWlsIiwicHJvZmlsZSIsInJvbGVzIiwiYXJzb3VuZHMucmVhZCIsImFyc291bmRzLndyaXRlIl0sImFtciI6WyJwd2QiXX0.FtzVVPw4S_zAinmK2RVoVxk5oUIm8Ivyb6mpE1qFOQ6WNWu0ZWSDs6aEFLNhOzUe2d98QxB5TVlU4wstgcmN9KE6de6WE4dSmn1OWbwK85ED0I5SniorGvwIFjY7ZA4q1hK2KfZC_WjzQceVJcZeJGyFPyRwQb-PMlT5eBtXrFkQwYIIxLegpCpW19Z-NFbF7WYHGTnTbAhmBpw6PUO0MSbFE8TyFCHcRL36sAeCgZiOXNKXc6n-aTB15_74nLgbHh2K9KseEQds4MHmG0eVuzyF-Hu6lh33MQNuGWReY9uS0fY6r3LStBQqpfCxhxspX_l8td12VmHRT4n1INxh6w");

        var response = await _httpClient.GetAsync($"api/targets/?page=1&size=100", cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseMessage<IEnumerable<Target>>>(responseContent);
    }

    public async Task<ResponseMessage<Target>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService?.Token.AccessToken
            ?? "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQyRkE5RUExODYyMDJGNjMyRjc0MjZCQTk3QzNBQTc4IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE3NDI5NDY3NTksImV4cCI6MTc0Mjk1MDM1OSwiaXNzIjoiaHR0cHM6Ly9zdHMuc2tvcnViYS5sb2NhbCIsImF1ZCI6ImFyc291bmRzLmFwaSIsImNsaWVudF9pZCI6ImJhNzdhNTUwLWI5NDItNDc4ZC1hODg4LWExZDAyMzIxZjhkOCIsInN1YiI6ImRhYWQ3YjI3LWIxNDAtNGM1OC1hZGI5LWUwMDYzNDE5ZmI5ZCIsImF1dGhfdGltZSI6MTc0MjkzNTk5OSwiaWRwIjoibG9jYWwiLCJuYW1lIjoiYWRtaW4iLCJyb2xlIjoiU3VwZXJBZG1pbiIsImp0aSI6Ijc1MUJENDZBMDE4QjdDMzNDRDM2NDlGMzRCOUY3MzFBIiwic2lkIjoiOTkwQTBBNTVFMjYyRUFFQzNBQTZEN0YxQUQyQTQxMjUiLCJpYXQiOjE3NDI5NDY3NTksInNjb3BlIjpbIm9wZW5pZCIsImVtYWlsIiwicHJvZmlsZSIsInJvbGVzIiwiYXJzb3VuZHMucmVhZCIsImFyc291bmRzLndyaXRlIl0sImFtciI6WyJwd2QiXX0.FtzVVPw4S_zAinmK2RVoVxk5oUIm8Ivyb6mpE1qFOQ6WNWu0ZWSDs6aEFLNhOzUe2d98QxB5TVlU4wstgcmN9KE6de6WE4dSmn1OWbwK85ED0I5SniorGvwIFjY7ZA4q1hK2KfZC_WjzQceVJcZeJGyFPyRwQb-PMlT5eBtXrFkQwYIIxLegpCpW19Z-NFbF7WYHGTnTbAhmBpw6PUO0MSbFE8TyFCHcRL36sAeCgZiOXNKXc6n-aTB15_74nLgbHh2K9KseEQds4MHmG0eVuzyF-Hu6lh33MQNuGWReY9uS0fY6r3LStBQqpfCxhxspX_l8td12VmHRT4n1INxh6w");

        var response = await _httpClient.GetAsync($"api/targets/{id}", cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseMessage<Target>>(responseContent);
    }
}
