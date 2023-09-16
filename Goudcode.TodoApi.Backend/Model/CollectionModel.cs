namespace Goudcode.TodoApi.Backend.Model
{
    public class CollectionModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public UserModel User { get; set; } = null!;
    }
}
