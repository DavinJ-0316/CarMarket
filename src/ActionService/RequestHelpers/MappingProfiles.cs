using ActionService.DTOs;
using ActionService.Entities;
using AutoMapper;
namespace ActionService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // the Items inside Auction Entity needs to be included in the mapper,
        // because there is no name called Item property in the AuctionDto
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>()
            // o means destination member, d.Item; s means CreateAuctionDto
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
        // .IncludeMembers and .ForMember Only check for the rest un mapped properties
        CreateMap<CreateAuctionDto, Item>();
    }
}
