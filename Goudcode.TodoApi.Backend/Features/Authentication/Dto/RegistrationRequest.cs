﻿namespace Goudcode.TodoApi.Backend.Features.Authentication.Dto
{
    public class RegistrationRequest
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }
}
