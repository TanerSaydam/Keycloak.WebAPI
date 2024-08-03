using Keycloak.WebAPI.DTOs;
using System.Text;
using System.Text.Json;
using TS.Result;

namespace Keycloak.WebAPI.Services;

public sealed class HttpService(
    IHttpClientFactory httpClientFactory,
    KeycloakService keycloakService)
{
    public async Task<Result<T>> GetAsync<T>(string endpoint, bool sendToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        if (sendToken)
        {
            string token = await keycloakService.GetTokenAsync(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        var response = await client.GetAsync(endpoint, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
            return Result<T>.Failure(message!.ErrorDescription);
        }

        var result = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<T>(result)!;
        return Result<T>.Succeed(data);
    }

    public async Task<Result<T>> PostAsync<T>(string endpoint, object body, bool sendToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        if (sendToken)
        {
            string token = await keycloakService.GetTokenAsync(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }


        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(endpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorMessage = await response.Content.ReadFromJsonAsync<KeycloakBadRequestErrorResponseDto>();
                return Result<T>.Failure(errorMessage!.ErrorMessage);
            }
            else
            {
                var message = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
                return Result<T>.Failure(message!.ErrorDescription);
            }
        }

        var result = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<T>(result)!;
        return Result<T>.Succeed(data);
    }

    public async Task<Result<T>> PutAsync<T>(string endpoint, object body, bool sendToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        if (sendToken)
        {
            string token = await keycloakService.GetTokenAsync(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }


        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(endpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
            return Result<T>.Failure(message!.ErrorDescription);
        }

        var result = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<T>(result)!;
        return Result<T>.Succeed(data);
    }
    public async Task<Result<T>> PostFormDataAsync<T>(string endpoint, KeyValuePair<string, string>[] body, bool sendToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        if (sendToken)
        {
            string token = await keycloakService.GetTokenAsync(cancellationToken);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        var response = await client.PostAsync(endpoint, new FormUrlEncodedContent(body), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>(cancellationToken);
            return Result<T>.Failure(error!.ErrorDescription);
        }

        var result = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<T>(result)!;
        return Result<T>.Succeed(data);
    }
}
