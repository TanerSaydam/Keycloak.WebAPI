using Keycloak.WebAPI.DTOs;
using Keycloak.WebAPI.Options;
using Keycloak.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace Keycloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController(
    KeycloakOptions keycloak,
    KeycloakService keycloakService,
    IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
    {
        var token = await keycloakService.GetToken(cancellationToken);
        KeycloakRegisterDto keycloakRegister = new(request);

        var enpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users";
        var client = httpClientFactory.CreateClient();

        var json = JsonSerializer.Serialize(keycloakRegister);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var response = await client.PostAsync(enpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
            return BadRequest(message);
        }

        return Ok(new { Message = "Register is successful" });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        var endPoint = $"{keycloak.AuthServerUrl}/realms/{keycloak.Realm}/protocol/openid-connect/token";
        var client = httpClientFactory.CreateClient();

        var data = new[]
        {
            new KeyValuePair<string, string>("username", request.UserName),
            new KeyValuePair<string, string>("password", request.Password),
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", keycloak.Resource),
            new KeyValuePair<string, string>("client_secret", keycloak.Credentials.Secret),
        };

        var response = await client.PostAsync(endPoint, new FormUrlEncodedContent(data), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
            return BadRequest(message);
        }

        LoginResponseDto? loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(response.Content.ReadAsStringAsync().Result);

        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.ReadToken(loginResponse!.access_token);
        return Ok(loginResponse);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string email, CancellationToken cancellationToken)
    {
        var token = await keycloakService.GetToken(cancellationToken);

        KeycloakUserDto user = await keycloakService.GetUserByEmail(email, token, cancellationToken);

        if (user.EmailVerified)
        {
            return BadRequest(new { Message = "Email already verified" });
        }

        var enpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{user.Id}";
        var client = httpClientFactory.CreateClient();
        object data = new
        {
            emailVerified = true
        };
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var response = await client.PutAsync(enpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadFromJsonAsync<KeycloakErrorResponseDto>();
            return BadRequest(message);
        }

        return Ok(new { Message = "Email was confirmed successfully" });
    }
}
