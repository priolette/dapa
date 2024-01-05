using AutoMapper;
using DAPA.Models.Public;

namespace DAPA.Models.Mappings;

public class LoyaltyProfile : Profile
{
    public LoyaltyProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<LoyaltyCreateRequest, Loyalty>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<LoyaltyFindRequest, Loyalty>();
        CreateMap<LoyaltyUpdateRequest, Loyalty>();
        CreateMap<int, Loyalty>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(x => x));
    }
}