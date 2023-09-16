using Goudcode.TodoApi.Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Goudcode.TodoApi.Backend.Features.Collections
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CollectionController
    {
        [HttpGet]
        public async Task<IResult> GetAll(TodoDataContext ctx)
        {
            var collections = await ctx.Collections.ToListAsync();
            return Results.Ok(collections);
        }
    }
}
