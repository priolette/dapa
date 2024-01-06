using AutoMapper;
using DAPA.Models.Public;
using DAPA.Models.Public.Loyalties;

namespace DAPA.Models.Mappings;

public class LoyaltyProfile : Profile
{
    public LoyaltyProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<LoyaltyCreateRequest, Loyalty>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(
            x => x.DiscountId, opt => opt.MapFrom(x => x.Discount)).ForMember(x => x.Discount, opt => opt.Ignore());
        CreateMap<LoyaltyFindRequest, Loyalty>();
        CreateMap<LoyaltyUpdateRequest, Loyalty>().ForMember(x => x.DiscountId, opt => opt.MapFrom(x => x.Discount));
        CreateMap<int, Loyalty>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(x => x));
    }
}