using Goudcode.TodoApi.Backend.Features.Authentication.Dto;
using Goudcode.TodoApi.Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
