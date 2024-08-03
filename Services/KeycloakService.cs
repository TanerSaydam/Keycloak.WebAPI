using Keycloak.WebAPI.DTOs;
using Keycloak.WebAPI.Options;
using System.Text.Json;

namespace Keycloak.WebAPI.Services;

public sealed class KeycloakService(
    KeycloakOptions keycloak,
    IHttpClientFactory httpClientFactory)
{
    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        var endPoint = $"{keycloak.AuthServerUrl}/realms/{keycloak.Realm}/protocol/openid-connect/token";
        var client = httpClientFactory.CreateClient();

        var data = new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", keycloak.Resource),
            new KeyValuePair<string, string>("client_secret", keycloak.Credentials.Secret),
        };

        var response = await client.PostAsync(endPoint, new FormUrlEncodedContent(data), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var objMessage = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
            string stringMessage = JsonSerializer.Serialize(objMessage);
            throw new ArgumentException(stringMessage);
        }

        LoginResponseDto? loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(response.Content.ReadAsStringAsync().Result);

        return loginResponse!.access_token;
    }

    public async Task<KeycloakUserDto> GetUserByEmail(string email, string token, CancellationToken cancellationToken)
    {
        var endPoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users?email={email}";
        var client = httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var response = await client.GetAsync(endPoint, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var objMessage = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
            string stringMessage = JsonSerializer.Serialize(objMessage);
            throw new ArgumentException(stringMessage);
        }

        var result = await response.Content.ReadAsStringAsync();

        List<KeycloakUserDto> keycloakUserDtos = JsonSerializer.Deserialize<List<KeycloakUserDto>>(result)!;

        if (keycloakUserDtos.Count == 0)
        {
            throw new ArgumentException("User not found");
        }

        KeycloakUserDto user = keycloakUserDtos.First();

        return user;
    }
}
