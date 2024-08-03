namespace Keycloak.WebAPI.Options;

public class KeycloakOptions
{
    public KeycloakOptions(IConfiguration configuration)
    {
        Realm = configuration.GetSection("Keycloak:realm").Value!;
        AuthServerUrl = configuration.GetSection("Keycloak:auth-server-url").Value!;
        SslRequired = configuration.GetSection("Keycloak:ssl-required").Value!;
        Resource = configuration.GetSection("Keycloak:resource").Value!;
        VerifyTokenAudience = Convert.ToBoolean(configuration.GetSection("Keycloak:verify-token-audience").Value!);
        Credentials = new() { Secret = configuration.GetSection("Keycloak:credentials:secret").Value! };
        UseResourceRoleMappings = Convert.ToBoolean(configuration.GetSection("Keycloak:use-resource-role-mappings").Value!);
        ConfidentialPort = Convert.ToInt32(configuration.GetSection("Keycloak:confidential-port").Value!);
        ClientUUID = configuration.GetSection("Keycloak:keycloak_client_uuid").Value!;
    }
    public string Realm { get; set; } = default!;
    public string AuthServerUrl { get; set; } = default!;
    public string SslRequired { get; set; } = default!;
    public string Resource { get; set; } = default!;
    public bool VerifyTokenAudience { get; set; } = default!;
    public Credentials Credentials { get; set; } = new();
    public bool UseResourceRoleMappings { get; set; }
    public int ConfidentialPort { get; set; }
    public PolicyEnforcer PolicyEnforcer { get; set; } = new();
    public string ClientUUID { get; set; } = default!;
}
