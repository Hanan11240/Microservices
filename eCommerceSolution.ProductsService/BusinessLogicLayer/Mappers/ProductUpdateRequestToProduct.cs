

using AutoMapper;
using DataAccessLayer.Entities;
using eCommerce.BusinessLogicLayer.DTO;

namespace eCommerce.BusinessLogicLayer.Mappers;
    public class ProductUpdateRequestToProduct:Profile
    {
    public ProductUpdateRequestToProduct()
    {

        CreateMap<ProductUpdateRequest, Product>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

    }
}

