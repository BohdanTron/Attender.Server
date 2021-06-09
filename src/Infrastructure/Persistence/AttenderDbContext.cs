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
    public sealed class AttenderDbContext : DbContext, IAttenderDbContext
    {
        public AttenderDbContext(DbContextOptions<AttenderDbContext> options)
            : base(options)
        {
            var connection = (SqlConnection) Database.GetDbConnection();
            var credential = new DefaultAzureCredential();

            var azureSqlScopes = new[] { "https://database.windows.net/.default" };
            var accessToken = credential.GetToken(new TokenRequestContext(azureSqlScopes));

            connection.AccessToken = accessToken.Token;
        }

        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<SubCategory> SubCategories => Set<SubCategory>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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
