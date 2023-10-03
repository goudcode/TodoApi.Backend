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
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CollectionResponse>))]
        public async Task<IResult> GetAll(TodoDataContext ctx)
        {
            var userId = User.FindFirstValue(IdentityData.UserIdClaimName);
            if (userId is null)
                return Results.Forbid();

            var collections = (await _collectionService
                .GetCollectionsByUser(new Guid(userId)))
                .Select(collection => new CollectionResponse()
                {
                    Id = collection.Id,
                    Name = collection.Name,
                    Description = collection.Description
                });

            return Results.Ok(collections);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CollectionResponse))]
        public async Task<IResult> Create(CreateCollectionRequest req, TodoDataContext ctx)
        {
            var userId = User.FindFirstValue(IdentityData.UserIdClaimName);
            if (userId is null)
                return Results.Forbid();

            var collection = await _collectionService.CreateCollection(
                req.Name, req.Description, new Guid(userId));

            return Results.Ok(new CollectionResponse()
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description
            });
        }
    }
}
