using System.Text.Json.Serialization;

namespace Keycloak.WebAPI.DTOs;

public sealed class KeycloakRegisterDto
{
    public KeycloakRegisterDto()
    {

    }
    public KeycloakRegisterDto(RegisterDto request)
    {
        KeycloakCredential credential = new()
        {
            Value = request.Password
        };

        Credentials = new() { credential };
        FirstName = request.FirstName;
        LastName = request.LastName;
        UserName = request.UserName;
        Email = request.Email;
        Attributes = new();
        Enabled = true;
    }

    [JsonPropertyName("attributes")]
    public KeycloakAttributes Attributes { get; set; } = new();

    [JsonPropertyName("credentials")]
    public List<KeycloakCredential> Credentials { get; set; } = new();

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = default!;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = default!;

    [JsonPropertyName("username")]
    public string UserName { get; set; } = default!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

}


public sealed class KeycloakAttributes
{
    [JsonPropertyName("attribute_key")]
    public string Attribute_Key { get; set; } = default!;
}

public sealed class KeycloakCredential
{
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = default!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = "password";

    [JsonPropertyName("value")]
    public string Value { get; set; } = "1";
}