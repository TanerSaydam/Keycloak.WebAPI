using System.Text.Json.Serialization;

namespace Keycloak.WebAPI.DTOs;

public sealed class ErrorResponseDto
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = default!;

    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; } = default!;
}


public sealed class KeycloakBadRequestErrorResponseDto
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = default!;

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; } = default!;
}

