using Attender.Server.Application.Cities.Queries;
using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Attender.Server.Application.Countries.Models
{
    public record CountryDto : IMapFrom<Country>
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public string Code { get; init; } = null!;

        [Required]
        public string Name { get; init; } = null!;

        [Required]
        public bool Supported { get; init; }

        public double? Distance { get; init; }

        [Required]
        public decimal Longitude { get; init; }

        [Required]
        public decimal Latitude { get; init; }

        [Required]
        public List<CityDto> Cities { get; init; } = null!;
    }
}
