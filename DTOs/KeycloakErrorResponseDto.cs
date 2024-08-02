using System.Text.Json.Serialization;

namespace Keycloak.WebAPI.DTOs;

public sealed class KeycloakErrorResponseDto
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = default!;

    [JsonPropertyName("error-description")]
    public string ErrorDescription { get; set; } = default!;
}
