using Attender.Server.Application.Cities.Queries;
using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using AutoMapper;
using System.Collections.Generic;

namespace Attender.Server.Application.Countries.Dtos
{
    public record CountryDto : IMapFrom<Country>
    {
        public int Id { get; init; }
        public string Code { get; init; } = null!;
        public string Name { get; init; } = null!;
        public IEnumerable<CityDto> Cities { get; set; } = new List<CityDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Country, CountryDto>()
                .ForMember(d => d.Cities, opt => opt.Ignore());
        }
    }
}
