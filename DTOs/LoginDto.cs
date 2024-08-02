using System.Text.Json.Serialization;

namespace Keycloak.WebAPI.DTOs;

public sealed class LoginDto
{
    [JsonPropertyName("username")]
    public string UserName { get; set; } = default!;
    [JsonPropertyName("password")]
    public string Password { get; set; } = default!;
}
