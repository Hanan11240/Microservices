using AutoMapper;
using DataAccessLayer.Entities;
using eCommerce.BusinessLogicLayer.DTO;

namespace eCommerce.BusinessLogicLayer.Mappers;
    public class ProductAddRequestToProductMappingProfile:Profile
    {
            public ProductAddRequestToProductMappingProfile() {

        CreateMap<ProductAddRequest, Product>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.ProductID, opt => opt.Ignore());

            }
    }

