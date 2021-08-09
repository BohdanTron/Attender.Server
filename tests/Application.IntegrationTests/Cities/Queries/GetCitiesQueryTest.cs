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
                var countries = new List<Country>
                {
                    new()
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
                    },
                    new()
                    {
                        Code = "en",
                        Name = "England",
                        Latitude = 0,
                        Longitude = 0,
                        Supported = false,
                        Cities = new List<City>()
                        {
                            new() {Name = "London"},
                            new() {Name = "Chernihiv England"}
                        }
                    },
                    new()
                    {
                        Code = "us",
                        Name = "USA",
                        Latitude = 0,
                        Longitude = 0,
                        Supported = false,
                        Cities = new List<City>()
                        {
                            new() {Name = "Washington"},
                            new() {Name = "Chernihiv USA"}
                        }
                    }
                };
                return _fixture.InsertRangeAsync(countries);
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
        public async Task ShouldReturnSpecificCitiesFromSupportedCountries(string cityName)
        {
            var supportedCountry = await _fixture.GetAsync<Country>(c => c.Code == "ua");
            
            var query = new GetCitiesQuery(cityName);
            var result = await _fixture.SendAsync(query);
            
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);
            result.All(c =>
                c.Name.Contains(cityName, StringComparison.InvariantCultureIgnoreCase) &&
                c.CountryId == supportedCountry.Id)
                .Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnAllCitiesFromSupportedCountries()
        {
            var supportedCountry = await _fixture.GetAsync<Country>(c => c.Code == "ua");
            
            var query = new GetCitiesQuery(null);
            var result = await _fixture.SendAsync(query);
            
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(6);
            result.All(c => c.CountryId == supportedCountry.Id).Should().BeTrue();
        }
    }
}
