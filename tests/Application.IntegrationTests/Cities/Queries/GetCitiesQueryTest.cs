using Attender.Server.Application.Cities.Queries.GetCities;
using Attender.Server.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Attender.Server.Application.IntegrationTests.Cities.Queries
{
    [Collection(nameof(SharedFixture))]
    public class GetCitiesQueryTest : IClassFixture<GetCitiesQueryTest.Fixture>
    {
        private readonly SharedFixture _fixture;

        public GetCitiesQueryTest(SharedFixture fixture)
            => _fixture = fixture;

        [Collection(nameof(SharedFixture))]
        public class Fixture : IAsyncLifetime
        {
            private readonly SharedFixture _fixture;

            public Fixture(SharedFixture fixture)
                => _fixture = fixture;

            public Task InitializeAsync()
            {
                var country = new Country
                {
                    Code = "ua",
                    Name = "Ukraine",
                    Latitude = 0,
                    Longitude = 0,
                    Supported = true,
                    Cities = new List<City>
                     {
                         new() {Name = "Chernihiv"},
                         new() {Name = "Lviv"},
                         new() {Name = "Kyiv"},
                         new() {Name = "Odessa"},
                         new() {Name = "Cherkasy"},
                         new() {Name = "Chernivtsi"}
                     }
                };
                return _fixture.InsertAsync(country);
            }

            public Task DisposeAsync()
            {
                return Task.CompletedTask;
            }
        }

        [Theory]
        [InlineData("Cher")]
        [InlineData("cher")]
        [InlineData("che")]
        public async Task ShouldReturnCitiesWhichContainInputName(string cityName)
        {
            // Act
            var query = new GetCitiesQuery(cityName);
            var result = await _fixture.SendAsync(query);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);
            result.All(c => c.Name.Contains(cityName, StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnEmptyList()
        {
            var query = new GetCitiesQuery(null);
            var result = await _fixture.SendAsync(query);

            result.Should().BeEmpty();
        }
    }
}
