
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;

namespace eCommerce.Infrastructure.Repository;

internal class UserRepository : IUserRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();
        return user;
    }

    public async  Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        return new ApplicationUser() { UserId = Guid.NewGuid(), Email = email , Password = password,PersonName = "Person A", Gender = GenderOptions.Male.ToString()};
    }
}

