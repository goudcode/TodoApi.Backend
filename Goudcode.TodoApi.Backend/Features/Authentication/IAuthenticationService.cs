using Goudcode.TodoApi.Backend.Features.Authentication.Dto.Service;
using Goudcode.TodoApi.Backend.Model;

namespace Goudcode.TodoApi.Backend.Features.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResult> VerifyAuthentication(string username, string password);
    Task<UserModel> RegisterNewUser(string username, string password);
}