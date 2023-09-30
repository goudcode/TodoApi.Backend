using Goudcode.TodoApi.Backend.Features.Authentication.Dto;
using Goudcode.TodoApi.Backend.Features.Authentication.Exceptions;
using Goudcode.TodoApi.Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Goudcode.TodoApi.Backend.Features.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<IResult> Register(RegistrationRequest req, TodoDataContext ctx)
        {
            try
            {
                await _authenticationService.RegisterNewUser(
                    req.Username, req.Password);
                return Results.Ok();
            }
            catch (UsernameAlreadyExistsException)
            {
                return Results.BadRequest("Username already in use");
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        public async Task<IResult> Login(LoginRequest req, TodoDataContext ctx, IConfiguration config)
        {
            var authenticationResult = await _authenticationService
                .VerifyAuthentication(req.Username, req.Password);
            if (!authenticationResult.IsAuthenticated)
                return Results.BadRequest("Authentication failed.");

            var response = new LoginResponse
            {
                JwtToken = authenticationResult.JwtBearer
            };

            return Results.Ok(response);
        }
    }
}