using AutoMapper;
using DAPA.Models.Public.Orders;

namespace DAPA.Models.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<OrderCreateRequest, Order>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<OrderFindRequest, Order>();
        CreateMap<OrderUpdateRequest, Order>();
        CreateMap<int, Order>().ForMember(x => x.Id, opt => opt.MapFrom(src => src));
    }
}