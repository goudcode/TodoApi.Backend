using Goudcode.TodoApi.Backend.Features.Authentication;
using Goudcode.TodoApi.Backend.Features.Collections.Dto;
using Goudcode.TodoApi.Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Goudcode.TodoApi.Backend.Features.Collections
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CollectionController : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetAll(TodoDataContext ctx)
        {
            var userId = User.FindFirstValue(IdentityData.UserIdClaimName);
            if (userId is null)
                return Results.Forbid();

            var collections = await ctx.Collections
                .Where(collection => collection.UserId.ToString() == userId)
                .ToListAsync();

            return Results.Ok(collections);
        }

        [HttpPost]
        public async Task<IResult> Create(CreateCollectionRequest req, TodoDataContext ctx)
        {
            var userId = User.FindFirstValue(IdentityData.UserIdClaimName);
            if (userId is null)
                return Results.Forbid();

            var userGuid = new Guid(userId);
            var collectionModel = new CollectionModel
            {
                Name = req.Name,
                Description = req.Description,
                UserId = userGuid
            };

            ctx.Collections.Add(collectionModel);
            await ctx.SaveChangesAsync();

            return Results.Ok(collectionModel);
        }
    }
}
