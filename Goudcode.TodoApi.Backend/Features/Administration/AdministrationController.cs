using Goudcode.TodoApi.Backend.Features.Administration.Dto;
using Goudcode.TodoApi.Backend.Features.Authentication;
using Goudcode.TodoApi.Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Goudcode.TodoApi.Backend.Features.Administration
{
    [ApiController]
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [Route("[controller]")]
    public class AdministrationController
    {
        [HttpGet]
        [Route("Users")]
        public async Task<IResult> GetUsers(TodoDataContext ctx)
        {
            var users = await ctx.Users.
                Select(user => new UserResponse()
                {
                    Id = user.Id,
                    Username = user.Username
                }).ToListAsync();

            return Results.Ok(users);
        }
    }
}
