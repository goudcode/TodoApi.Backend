using Goudcode.TodoApi.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Goudcode.TodoApi.Backend.Features.Administration;

public class AdministrationService : IAdministrationService
{
    private readonly TodoDataContext _todoDataContext;

    public AdministrationService(TodoDataContext todoDataContext)
    {
        _todoDataContext = todoDataContext;
    }

    public async Task<List<UserModel>> GetUsers()
        => await _todoDataContext.Users.ToListAsync();
}