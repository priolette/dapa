using AutoMapper;
using AutoMapper.Execution;
using DAPA.Models.Public.Products;

namespace DAPA.Models.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        MapRequests();
    }

    private void MapRequests()
    {
        CreateMap<ProductCreateRequest, Product>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ProductFindRequest, Product>();
        CreateMap<ProductUpdateRequest, Product>();
        CreateMap<int, Product>(MemberList.None).ForMember(x => x.Id, opt => opt.MapFrom(src => src));
    }
}