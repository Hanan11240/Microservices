
using Dapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repository;

internal class UserRepository : IUserRepository
{

    private readonly DapperDbContext _dbContext;

    public UserRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();
        // Query  to insert user data into Users
        string query = "Insert into public.\"Users\"(\"UserID\",\"Email\", \"PersonName\",\"Gender\",\"Password\") " +
            $"Values(@UserID,@Email,@PersonName,@Gender,@Password) ";
      int rowCount =  await _dbContext.DbConnection.ExecuteAsync(query, user);
        if(rowCount > 0)
        {
            return user;
        }
        return null;
    }

    public async  Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        string query = "Select * from public.\"Users\" where \"Email\"=@Email and \"Password\" = @Password";
      
      ApplicationUser? user =  await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, new {Email = email,Password= password});

        if(user is null)
        {
            return null;
        }
        return user;
    }
}

