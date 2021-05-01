using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Attender.Server.Application.Countries.Queries.GetCountry
{
    public record CountryDto : IMapFrom<Country>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public bool Supported { get; set; }

        public double? Distance { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public decimal Latitude { get; set; }
    }
}
