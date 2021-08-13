using Attender.Server.Application.SubCategories.Queries.GetSubCategories;
using Attender.Server.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Attender.Server.Application.IntegrationTests.SubCategories.Queries
{
    [Collection(nameof(SharedFixture))]
    public class GetSubCategoriesQueryTest : IClassFixture<GetSubCategoriesQueryTest.Fixture>
    {
        private readonly SharedFixture _fixture;

        public GetSubCategoriesQueryTest(SharedFixture fixture)
            => _fixture = fixture;

        [Collection(nameof(SharedFixture))]
        public class Fixture : IAsyncLifetime
        {
            private readonly SharedFixture _fixture;

            public Fixture(SharedFixture fixture)
                => _fixture = fixture;

            public Task InitializeAsync()
            {
                var categories = new List<Category>
                {
                    new()
                    {
                        Name = "Music",
                        SubCategories = new List<SubCategory>
                        {
                            new() {Name = "Rock"},
                            new() {Name = "Hip-Hop"},
                            new() {Name = "Jazz"},
                            new() {Name = "Metal"},
                            new() {Name = "Pop1"},
                            new() {Name = "Pop12"},
                            new() {Name = "Pop13"}
                        }
                    },
                    new()
                    {
                        Name = "Sports",
                        SubCategories = new List<SubCategory>
                        {
                            new() {Name = "Football"},
                            new() {Name = "Volleyball"},
                            new() {Name = "Tennis"}
                        }
                    }
                };

                return _fixture.InsertRangeAsync(categories);
            }

            public Task DisposeAsync()
            {
                return Task.CompletedTask;
            }
        }

        [Theory]
        [InlineData("Po")]
        [InlineData("Pop")]
        [InlineData("Pop1")]
        public async Task ShouldReturnSpecificSubCategories(string subCategoryName)
        {
            var category = await _fixture.GetAsync<Category>(c => c.Name == "Music");

            var query = new GetSubCategoriesQuery(subCategoryName);
            var result = await _fixture.SendAsync(query);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);
            result.All(c =>
                c.Name.Contains(subCategoryName, StringComparison.InvariantCultureIgnoreCase) &&
                c.CategoryId == category.Id).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnAllSubCategories()
        {
            var query = new GetSubCategoriesQuery(null);
            var result = await _fixture.SendAsync(query);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(10);
        }
    }
}
