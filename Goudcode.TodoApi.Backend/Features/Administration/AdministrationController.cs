using Goudcode.TodoApi.Backend.Features.Administration.Dto;
using Goudcode.TodoApi.Backend.Features.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goudcode.TodoApi.Backend.Features.Administration
{
    [ApiController]
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [Route("[controller]")]
    public class AdministrationController
    {
        private readonly IAdministrationService _administrationService;

        public AdministrationController(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }
        
        [HttpGet]
        [Route("Users")]
        public async Task<IResult> GetUsers()
        {
            var users = (await _administrationService.GetUsers())
                .Select(user => new UserResponse()
                {
                    Id = user.Id,
                    Username = user.Username
                });

            return Results.Ok(users);
        }
    }
}
