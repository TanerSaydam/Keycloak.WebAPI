using System.Text.Json.Serialization;

namespace Keycloak.WebAPI.DTOs;

public sealed class SetOrDeleteRoleDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
}
