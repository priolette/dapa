using AutoMapper;
using DAPA.Models.Public.Reservations;

namespace DAPA.Models.Mappings;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<ReservationCreateRequest, Reservation>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ReservationFindRequest, Reservation>();
        CreateMap<ReservationUpdateRequest, Reservation>();
        CreateMap<int, Reservation>(MemberList.None).ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
    }
}