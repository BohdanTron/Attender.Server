﻿using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface IAttenderDbContext
    {
        DbSet<Artist> Artists { get; }
        DbSet<Category> Categories { get; }
        DbSet<City> Cities { get; }
        DbSet<Country> Countries { get; }
        DbSet<Event> Events { get; }
        DbSet<Location> Locations { get; }
        DbSet<Role> Roles { get; }
        DbSet<SubCategory> SubCategories { get; }
        DbSet<Ticket> Tickets { get; }
        DbSet<User> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<Language> Languages { get; }
        DbSet<CategoryDescription> CategoryDescriptions { get; }
        DbSet<EventSection> EventSections { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    }
}
