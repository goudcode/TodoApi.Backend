namespace Goudcode.TodoApi.Backend.Features.Collections.Dto
{
    public class CreateCollectionRequest
    {
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
    }
}
