using DataAccessLayer.Repositries;
using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,IConfiguration configuration)
        {
            // Add Data access layer services into Ioc Container
            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
                }
                );
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
