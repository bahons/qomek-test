using auth.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace auth.DbContext;


public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    protected readonly IConfiguration Configuration;

    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Configuration.GetConnectionString("DbConString"));
    }


    public DbSet<User> Users { get; set; }
}
