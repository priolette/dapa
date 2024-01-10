using AutoMapper;
using DAPA.Models.Public.Payments;

namespace DAPA.Models.Mappings;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<PaymentCreateRequest, Payment>().ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Order, opt => opt.Ignore());
        CreateMap<PaymentFindRequest, Payment>();
        CreateMap<PaymentUpdateRequest, Payment>();
        CreateMap<int, Payment>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(src => src));
    }
}