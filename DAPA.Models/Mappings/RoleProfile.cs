using AutoMapper;
using DAPA.Models.Public.Roles;

namespace DAPA.Models.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<RoleCreateRequest, Role>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<RoleUpdateRequest, Role>();
        CreateMap<RoleFindRequest, Role>();
        CreateMap<int, Role>().ForMember(x => x.Id, opt => opt.MapFrom(x => x));
    }
}