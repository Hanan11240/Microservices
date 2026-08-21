

using eCommerce.BusinessLogicLayer.Mappers;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.BusinessLogicLayer.Services;
using eCommerce.BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer
{
    public static class DependencyINjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {

            // Add Businesslayer services to IoC container
           
            services.AddAutoMapper(cfg => {
                cfg.AddMaps(typeof(ProductAddRequestToProductMappingProfile).Assembly);
            });
            services.AddScoped<IProductService, ProductService>();
            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
            return services;
        }
    }
}
