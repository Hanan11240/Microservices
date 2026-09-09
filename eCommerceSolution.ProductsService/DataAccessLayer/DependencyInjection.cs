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

            string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
           string connectionString= connectionStringTemplate.Replace("$MYSQL_HOST",Environment.GetEnvironmentVariable("MYSQL_HOST")!)
            .Replace("$MYSQL_PASSWORD",Environment.GetEnvironmentVariable("MYSQL_PASSWORD")!)
            .Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE")!)
            .Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER")!)
            .Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT")!);
            // Add Data access layer services into Ioc Container
            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseMySQL(connectionString);
                }
                );
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
