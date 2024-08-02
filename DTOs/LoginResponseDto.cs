namespace Keycloak.WebAPI.DTOs;

public class LoginResponseDto
{
    public string access_token { get; set; } = default!;
    public int expires_in { get; set; } = default!;
    public int refresh_expires_in { get; set; }
    public string refresh_token { get; set; } = default!;
    public string token_type { get; set; } = default!;
    public int notbeforepolicy { get; set; }
    public string session_state { get; set; } = default!;
    public string scope { get; set; } = default!;
}
