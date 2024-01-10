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

        CreateMap<ProductCartCreateRequest, ProductCart>().ForMember(x => x.Order, opt => opt.Ignore())
            .ForMember(x => x.Product, opt => opt.Ignore());
        CreateMap<ProductCartFindRequest, ProductCart>();
        CreateMap<ProductCartUpdateRequest, ProductCart>();
        CreateMap<(int, int), ProductCart>(MemberList.None)
            .ForMember(x => x.OrderId, opt => opt.MapFrom(src => src.Item1))
            .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.Item2));
    }
}