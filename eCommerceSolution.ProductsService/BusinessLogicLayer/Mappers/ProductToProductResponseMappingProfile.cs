using AutoMapper;
using DataAccessLayer.Entities;
using eCommerce.BusinessLogicLayer.DTO;


namespace eCommerce.BusinessLogicLayer.Mappers;
    public class ProductToProductResponseMappingProfile:Profile
    {
        public ProductToProductResponseMappingProfile()
        {

            CreateMap<Product, ProductResponse>()
                .ForCtorParam("Category", opt => opt.MapFrom(src => ParseCategory(src.Category)))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => ParseCategory(src.Category)));
        }

        private static CategoryOptions ParseCategory(string category)
        {
            if (string.IsNullOrEmpty(category)) return default;
            if (Enum.TryParse<CategoryOptions>(category, true, out var result)) return result;
            return default;
        }
    }

