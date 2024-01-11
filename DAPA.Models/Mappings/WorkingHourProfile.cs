using AutoMapper;
using DAPA.Models.Public;
using DAPA.Models.Public.WorkingHours;

namespace DAPA.Models.Mappings;

public class WorkingHourProfile : Profile
{
    public WorkingHourProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<WorkingHoursCreateRequest, WorkingHour>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<WorkingHoursFindRequest, WorkingHour>();
        CreateMap<WorkingHoursUpdateRequest, WorkingHour>();
        CreateMap<int, WorkingHour>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(x => x));
    }
}