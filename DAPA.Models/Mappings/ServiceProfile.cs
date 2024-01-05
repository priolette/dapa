using AutoMapper;
using DAPA.Models.Public;

namespace DAPA.Models.Mappings;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<ServiceCreateRequest, Service>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ServiceFindRequest, Service>();
        CreateMap<ServiceUpdateRequest, Service>();
        CreateMap<int, Service>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(x => x));
    }
}