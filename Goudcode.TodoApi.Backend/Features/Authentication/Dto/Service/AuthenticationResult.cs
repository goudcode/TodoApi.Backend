namespace Goudcode.TodoApi.Backend.Features.Authentication.Dto.Service;

public class AuthenticationResult
{
    public bool IsAuthenticated { get; init; }
    public string JwtBearer { get; init; } = string.Empty;
}