using Goudcode.TodoApi.Backend.Features.Authentication;
using Goudcode.TodoApi.Backend.Features.Authentication.Dto;
using Goudcode.TodoApi.Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Goudcode.TodoApi.Backend.Usecases.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController
    {
        [HttpPost("Register")]
        public async Task<IResult> Register(RegistrationRequest req, TodoDataContext ctx)
        {
            // Check if user already exists
            var existingUser = await ctx.Users.FirstOrDefaultAsync(user => user.Username == req.Username.ToLower());
            if (existingUser is not null)
                return Results.BadRequest("Username already in use");

            // Create user with secure password hash
            var user = new UserModel();
            var hasher = new PasswordHasher<UserModel>();

            user.Username = req.Username.ToLower();
            user.Password = hasher.HashPassword(user, req.Password);
           
            ctx.Add(user);
            await ctx.SaveChangesAsync();
            
            return Results.Ok();
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        public async Task<IResult> Login(LoginRequest req, TodoDataContext ctx, IConfiguration config)
        {
            // Check if user exists
            var existingUser = await ctx.Users.FirstOrDefaultAsync(user => user.Username == req.Username.ToLower());
            if (existingUser is null)
                return Results.BadRequest("Invalid Login");

            // Hash password
            var hasher = new PasswordHasher<UserModel>();
            var hashResult = hasher.VerifyHashedPassword(existingUser, existingUser.Password, req.Password);

            // Verify password
            if (hashResult == PasswordVerificationResult.Failed)
                return Results.BadRequest("Invalid Login");

            // Rehash password if needed
            if (hashResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                var newPasswordHash = hasher.HashPassword(existingUser, req.Password);
                existingUser.Password = newPasswordHash;
                await ctx.SaveChangesAsync();
            }

            // Generate Token
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? 
                throw new Exception("Jwt Key Not configured")));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, existingUser.Username)
            };

            if (existingUser.IsAdmin)
                claims.Add(new Claim(IdentityData.AdminUserClaimName, "true"));

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: signingCredentials);

            var response = new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return Results.Ok(response);
        }
    }
}
