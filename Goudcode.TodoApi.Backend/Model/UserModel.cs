namespace Goudcode.TodoApi.Backend.Model
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;

        public ICollection<CollectionModel> Collections { get; set; } = new List<CollectionModel>();
    }
}
