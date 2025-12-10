using Microservice.IdentityServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservice.IdentityServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
