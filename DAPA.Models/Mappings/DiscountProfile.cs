using AutoMapper;
using DAPA.Models.Public;

namespace DAPA.Models.Mappings;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<DiscountCreateRequest, Discount>().ForMember(x => x.ID, opt => opt.Ignore());
        CreateMap<DiscountFindRequest, Discount>();
        CreateMap<DiscountUpdateRequest, Discount>();
        CreateMap<int, Discount>().ForMember(x => x.ID, opt => opt.MapFrom(x => x));
    }
}