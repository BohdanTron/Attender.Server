using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using AutoMapper;
using System.Collections.Generic;

namespace Attender.Server.Application.Countries
{
    public record CountryDto : IMapFrom<Country>
    {
        public int Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public IEnumerable<CityDto> Cities { get; set; } = new List<CityDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Country, CountryDto>()
                .ForMember(d => d.Cities, opt => opt.Ignore());
        }
    }

    public record CityDto
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public int CountryId { get; init; }
    }
}
