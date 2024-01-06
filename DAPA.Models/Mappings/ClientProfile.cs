using AutoMapper;
using DAPA.Models.Public.Clients;

namespace DAPA.Models.Mappings;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<ClientCreateRequest, Client>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ClientFindRequest, Client>();
        CreateMap<ClientUpdateRequest, Client>();
        CreateMap<int, Client>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(x => x));
    }
}