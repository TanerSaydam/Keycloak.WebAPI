using System.Text.Json.Serialization;

namespace Keycloak.WebAPI.DTOs;

public sealed class KeycloakRoleDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("composite")]
    public bool Composite { get; set; }

    [JsonPropertyName("clientRole")]
    public bool ClientRole { get; set; }

    [JsonPropertyName("containerId")]
    public Guid ContainerId { get; set; }
}
