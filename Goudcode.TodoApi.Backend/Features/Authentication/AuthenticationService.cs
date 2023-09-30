using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Goudcode.TodoApi.Backend.Features.Authentication.Dto.Service;
using Goudcode.TodoApi.Backend.Features.Authentication.Exceptions;
using Goudcode.TodoApi.Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Goudcode.TodoApi.Backend.Features.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly TodoDataContext _todoDataContext;
    private readonly IConfiguration _configuration;

    public AuthenticationService(TodoDataContext todoDataContext, IConfiguration configuration)
    {
        _todoDataContext = todoDataContext;
        _configuration = configuration;
    }

    public async Task<AuthenticationResult> VerifyAuthentication(string username, string password)
    {
        // Check if user exists
        var existingUser = await _todoDataContext.Users
            .FirstOrDefaultAsync(user => user.Username == username.ToLower());
        if (existingUser is null)
            return new AuthenticationResult() { IsAuthenticated = false };

        // Hash password
        var hasher = new PasswordHasher<UserModel>();
        var hashResult = hasher.VerifyHashedPassword(
            existingUser, existingUser.Password, password);

        bool isAuthenticated;
        switch (hashResult)
        {
            // Rehash password if needed
            case PasswordVerificationResult.SuccessRehashNeeded:
            {
                var newPasswordHash = hasher.HashPassword(existingUser, password);
                existingUser.Password = newPasswordHash;
                await _todoDataContext.SaveChangesAsync();
                isAuthenticated = true;
                break;
            }

            case PasswordVerificationResult.Success:
                isAuthenticated = true;
                break;

            case PasswordVerificationResult.Failed:
            default:
                isAuthenticated = false;
                break;
        }

        var token = isAuthenticated
            ? GenerateTokenForUser(existingUser.Id, existingUser.Username, existingUser.IsAdmin)
            : string.Empty;
        
        return new AuthenticationResult()
        {
            IsAuthenticated = isAuthenticated,
            JwtBearer = token
        };
    }

    public async Task<UserModel> RegisterNewUser(string username, string password)
    {
        // Check if user already exists
        var existingUser = await _todoDataContext.Users
            .FirstOrDefaultAsync(user => user.Username == username.ToLower());
        if (existingUser is not null)
            // TODO, Not really happy with the exception approach, find another way.
            throw new UsernameAlreadyExistsException();

        // Create user with secure password hash
        var user = new UserModel();
        var hasher = new PasswordHasher<UserModel>();

        user.Username = username.ToLower();
        user.Password = hasher.HashPassword(user, password);

        _todoDataContext.Add(user);
        await _todoDataContext.SaveChangesAsync();
        return user;
    }

    private string GenerateTokenForUser(Guid userId, string username, bool isAdmin)
    {
        // Generate Token
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ??
                                   throw new Exception("Jwt Key Not configured")));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(IdentityData.UsernameClaimName, username),
            new(IdentityData.UserIdClaimName, userId.ToString())
        };

        if (isAdmin)
            claims.Add(new Claim(IdentityData.AdminUserClaimName, "true"));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}