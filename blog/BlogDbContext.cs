using blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace blog
{
    public class BlogDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public BlogDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("DbConString"));
        }

        public DbSet<Post> Posts { get; set; }
    }
}
