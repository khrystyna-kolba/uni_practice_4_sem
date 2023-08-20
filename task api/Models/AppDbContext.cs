using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ContainersApiTask.Models.Containers;

namespace ContainersApiTask.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Container> Containers { get; set; }
        public DbSet<PublishedContainer> PublishedContainers { get; set; }
    }
}

