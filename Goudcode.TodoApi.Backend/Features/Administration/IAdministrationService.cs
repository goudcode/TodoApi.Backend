using Goudcode.TodoApi.Backend.Model;

namespace Goudcode.TodoApi.Backend.Features.Administration;

public interface IAdministrationService
{
    Task<List<UserModel>> GetUsers();
}