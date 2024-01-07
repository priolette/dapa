using AutoMapper;
using DAPA.Models.Public.Staff;

namespace DAPA.Models.Mappings;

public class StaffProfile : Profile
{
    public StaffProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<StaffCreateRequest, Staff>().ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Role, opt => opt.Ignore());
        CreateMap<StaffFindRequest, Staff>();
        CreateMap<StaffUpdateRequest, Staff>();
        CreateMap<int, Staff>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(x => x));
    }
}