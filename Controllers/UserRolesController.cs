using Keycloak.WebAPI.DTOs;
using Keycloak.WebAPI.Options;
using Keycloak.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class UserRolesController(
    HttpService httpService,
    KeycloakOptions keycloak) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUserRoles(Guid userId, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{userId}/role-mappings";

        var response = await httpService.GetAsync<object>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserRolesInRealm(Guid userId, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{userId}/role-mappings/realm";

        var response = await httpService.GetAsync<object>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserRolesInClient(Guid userId, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{userId}/role-mappings/clients/{keycloak.ClientUUID}";

        var response = await httpService.GetAsync<object>(endpoint, true, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }


    [HttpPost]
    public async Task<IActionResult> AssignmentRoleByUserId(Guid userId, List<SetOrDeleteRoleDto> request, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{userId}/clients/{keycloak.ClientUUID}";

        var response = await httpService.PostAsync<string>(endpoint, request, true, cancellationToken);

        response.Data = "Role assigned to user successfully";

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRoleByUserId(Guid userId, List<SetOrDeleteRoleDto> request, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{userId}/clients/{keycloak.ClientUUID}";

        var response = await httpService.DeleteAsync<string>(endpoint, request, true, cancellationToken);

        response.Data = "Roles were successfully removed from the user";

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllRoleByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var endpoint = $"{keycloak.AuthServerUrl}/admin/realms/{keycloak.Realm}/users/{userId}/clients/{keycloak.ClientUUID}";

        var response = await httpService.DeleteAsync<string>(endpoint, true, cancellationToken);

        response.Data = "All roles were successfully removed from the user";

        return StatusCode(response.StatusCode, response);
    }
}
