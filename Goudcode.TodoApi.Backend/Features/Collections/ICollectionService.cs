using Goudcode.TodoApi.Backend.Model;

namespace Goudcode.TodoApi.Backend.Features.Collections;

public interface ICollectionService
{
    public Task<IEnumerable<CollectionModel>> GetCollectionsByUser(Guid id);
    Task<CollectionModel> CreateCollection(string name, string description, Guid userId);
}