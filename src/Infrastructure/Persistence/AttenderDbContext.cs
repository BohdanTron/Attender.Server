using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Attender.Server.Infrastructure.Persistence
{
    public class AttenderDbContext : DbContext, IAttenderDbContext
    {
        public AttenderDbContext()
        {
        }

        public AttenderDbContext(DbContextOptions<AttenderDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artists => Set<Artist>();
        public virtual DbSet<Category> Categories => Set<Category>();
        public virtual DbSet<City> Cities => Set<City>();
        public virtual DbSet<Country> Countries => Set<Country>();
        public virtual DbSet<Event> Events => Set<Event>();
        public virtual DbSet<Location> Locations => Set<Location>();
        public virtual DbSet<Role> Roles => Set<Role>();
        public virtual DbSet<SubCategory> SubCategories => Set<SubCategory>();
        public virtual DbSet<Ticket> Tickets => Set<Ticket>();
        public virtual DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AttenderDbContext).Assembly);
        }
    }
}
