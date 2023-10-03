using Goudcode.TodoApi.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Goudcode.TodoApi.Backend.Features.Collections;

public class CollectionService : ICollectionService
{
    private readonly TodoDataContext _dataContext;

    public CollectionService(TodoDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<CollectionModel>> GetCollectionsByUser(Guid id)
    {
        return await _dataContext.Collections
            .Where(collection => collection.UserId == id)
            .ToListAsync();
    }

    public async Task<CollectionModel> CreateCollection(string name, string description, Guid userId)
    {
        var collection = new CollectionModel()
        {
            Name = name,
            Description = description,
            UserId = userId
        };

        _dataContext.Collections.Add(collection);
        await _dataContext.SaveChangesAsync();

        return collection;
    }
}