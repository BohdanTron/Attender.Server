using Attender.Server.API;
using Attender.Server.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Threading.Tasks;
using Xunit;

namespace Attender.Server.Application.IntegrationTests
{
    [CollectionDefinition(nameof(SharedFixture))]
    public class SharedFixtureCollection : ICollectionFixture<SharedFixture> { }

    public class SharedFixture : IAsyncLifetime
    {
        private readonly Checkpoint _checkpoint;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly WebApplicationFactory<Startup> _webAppFactory;

        public SharedFixture()
        {
            _webAppFactory = new AttenderWebApplicationFactory<Startup>();

            _configuration = _webAppFactory.Services.GetRequiredService<IConfiguration>();
            _scopeFactory = _webAppFactory.Services.GetRequiredService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint();
        }

        public async Task InsertAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            await using var context = scope.ServiceProvider.GetRequiredService<AttenderDbContext>();

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(request);
        }

        public Task InitializeAsync()
            => _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));

        public Task DisposeAsync()
        {
            _webAppFactory?.Dispose();
            return Task.CompletedTask;
        }
    }
}
