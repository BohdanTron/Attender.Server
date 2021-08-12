using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Domain.Entities;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Attender.Server.Infrastructure.Persistence
{
    public partial class AttenderDbContext : DbContext, IAttenderDbContext
    {
        public AttenderDbContext()
        {
        }

        public AttenderDbContext(DbContextOptions<AttenderDbContext> options)
            : base(options)
        {
            var connection = (SqlConnection) Database.GetDbConnection();
            if (connection.DataSource != null && connection.DataSource.Contains("database.windows.net"))
            {
                var credential = new DefaultAzureCredential();

                var azureSqlScopes = new[] { "https://database.windows.net/.default" };
                var accessToken = credential.GetToken(new TokenRequestContext(azureSqlScopes));

                connection.AccessToken = accessToken.Token;
            }
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
        public virtual DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public virtual DbSet<Language> Languages => Set<Language>();
        public virtual DbSet<CategoryDescription> CategoryDescriptions => Set<CategoryDescription>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AttenderDbContext).Assembly);
        }
    }
}
