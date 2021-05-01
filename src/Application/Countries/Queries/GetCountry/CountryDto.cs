using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;

namespace Attender.Server.Application.Countries.Queries.GetCountry
{
    public record CountryDto : IMapFrom<Country>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Supported { get; set; }
        public double? Distance { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
