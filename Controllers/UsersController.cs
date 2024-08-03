using Keycloak.WebAPI.DTOs;
using Keycloak.WebAPI.Options;
using Keycloak.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public sealed class UsersController(
    HttpService httpService,
    KeycloakService keycloakService,
    KeycloakOptions keycloak) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users";
        var response = await httpService.GetAsync<List<UserDto>>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{id}";
        var response = await httpService.GetAsync<UserDto>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users?email={email}";
        var response = await httpService.GetAsync<List<UserDto>>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserUpdateDto request, CancellationToken cancellationToken)
    {
        var token = await keycloakService.GetTokenAsync(cancellationToken);

        UserDto user = await keycloakService.GetUserById(request.Id, token, cancellationToken);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        if (user.Email != request.Email)
        {
            user.EmailVerified = false;
        }

        user.Email = request.Email;


        var enpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{user.Id}";

        var response = await httpService.PutAsync<string>(enpoint, user, true, cancellationToken);

        return Ok(new { Message = "User was update successfully" });
    }
}
