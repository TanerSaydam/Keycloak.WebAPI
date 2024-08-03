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
        var response = await httpService.GetAsync<List<KeycloakRoleDto>>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
