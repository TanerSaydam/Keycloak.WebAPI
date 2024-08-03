using Keycloak.WebAPI.DTOs;
using Keycloak.WebAPI.Options;
using Keycloak.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public sealed class RolesController(
    HttpService httpService,
    KeycloakOptions keycloak) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/clients/{keycloak.ClientUUID}/roles";
        var response = await httpService.GetAsync<List<RoleDto>>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/clients/{keycloak.ClientUUID}/roles/{name}";
        var response = await httpService.GetAsync<RoleDto>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/clients/{keycloak.ClientUUID}/roles";
        var data = new { name = name };
        var response = await httpService.PostAsync<string>(endpoint, data, true, cancellationToken);

        response.Data = "Role was created successfully";

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteByName(string name, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/clients/{keycloak.ClientUUID}/roles/{name}";
        var response = await httpService.DeleteAsync<string>(endpoint, true, cancellationToken);

        response.Data = "Role was deleted successfully";

        return StatusCode(response.StatusCode, response);
    }
}
