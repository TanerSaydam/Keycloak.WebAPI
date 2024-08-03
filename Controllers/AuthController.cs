using Keycloak.WebAPI.DTOs;
using Keycloak.WebAPI.Options;
using Keycloak.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController(
    KeycloakOptions keycloak,
    HttpService httpService,
    KeycloakService keycloakService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
    {
        KeycloakRegisterDto keycloakRegister = new(request);

        var enpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users";


        var response = await httpService.PostAsync<string>(enpoint, keycloakRegister, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        var endPoint = $"{keycloak.AuthServerUrl}/realms/{keycloak.Realm}/protocol/openid-connect/token";

        var data = new[]
       {
            new KeyValuePair<string, string>("username", request.UserName),
            new KeyValuePair<string, string>("password", request.Password),
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", keycloak.Resource),
            new KeyValuePair<string, string>("client_secret", keycloak.Credentials.Secret),
        };

        var response = await httpService.PostFormDataAsync<LoginResponseDto>(endPoint, data, false, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    public async Task<IActionResult> ConfirmEmail(string email, CancellationToken cancellationToken)
    {
        var token = await keycloakService.GetTokenAsync(cancellationToken);

        UserDto user = await keycloakService.GetUserByEmail(email, token, cancellationToken);

        if (user.EmailVerified)
        {
            return BadRequest(new { Message = "Email already verified" });
        }


        var enpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{user.Id}";

        object data = new
        {
            emailVerified = true
        };


        var response = await httpService.PutAsync<string>(enpoint, data, true, cancellationToken);

        return Ok(new { Message = "Email was confirmed successfully" });
    }

}