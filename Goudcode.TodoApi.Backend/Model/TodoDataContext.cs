using Microsoft.EntityFrameworkCore;

namespace Goudcode.TodoApi.Backend.Model
{
    public class TodoDataContext : DbContext
    {
        private readonly IConfiguration configuration;

        public TodoDataContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql(
                configuration.GetConnectionString("TodoDatabase") ??
                throw new Exception("Connection string TodoDatabase not set."));
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<CollectionModel> Collections { get; set; }
    }
}
