using AutoMapper;
using DAPA.Models.Public.Services;

namespace DAPA.Models.Mappings;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<ServiceCreateRequest, Service>().ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Discount, opt => opt.Ignore());
        CreateMap<ServiceFindRequest, Service>();
        CreateMap<ServiceUpdateRequest, Service>();
        CreateMap<int, Service>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(x => x));

        CreateMap<ServiceCartCreateRequest, ServiceCart>()
            .ForMember(x => x.Service, opt => opt.Ignore())
            .ForMember(x => x.Order, opt => opt.Ignore());
        CreateMap<ServiceCartFindRequest, ServiceCart>();
        CreateMap<ServiceCartUpdateRequest, ServiceCart>();
        CreateMap<(int, int), ServiceCart>(MemberList.None)
            .ForMember(x => x.OrderId, opt => opt.MapFrom(x => x.Item1))
            .ForMember(x => x.ServiceId, opt => opt.MapFrom(x => x.Item2));
    }
}