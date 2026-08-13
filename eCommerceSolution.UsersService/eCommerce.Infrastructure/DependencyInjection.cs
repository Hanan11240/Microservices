using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using eCommerce.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.Infrastructure;
    public static  class DependencyInjection
    {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<DapperDbContext>();
        return services;
    }
    }

