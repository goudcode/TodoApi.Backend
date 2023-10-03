using Goudcode.TodoApi.Backend.Features.Authentication;
using Goudcode.TodoApi.Backend.Model;
using Goudcode.TodoApi.Backend.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Goudcode.TodoApi.Backend.Features.Administration;
using Goudcode.TodoApi.Backend.Features.Collections;
using AdministrationService = Goudcode.TodoApi.Backend.Features.Administration.AdministrationService;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config["Jwt:Key"] ?? throw new Exception("Jwt Key not configured")))
        };
    });

services.AddAuthorization(opts =>
{
    opts.AddPolicy(IdentityData.AdminUserPolicyName,
        policy => { policy.RequireClaim(IdentityData.AdminUserClaimName, "true"); });
});

services.AddDbContext<TodoDataContext>();
services.AddTransient<IAdministrationService, AdministrationService>();
services.AddTransient<IAuthenticationService, AuthenticationService>();
services.AddTransient<ICollectionService, CollectionService>();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

services.AddSwaggerGen();
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();