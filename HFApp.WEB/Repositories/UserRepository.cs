using HFApp.WEB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HFApp.WEB.Repositories
{
    public class UserRepository :IUserRepository
    {
        
        
        private readonly AuthDbContext _authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }


        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await _authDbContext.Users.ToListAsync();

            var superAdminUser = await _authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "sa@email.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }

            return users;
        }
    }
}
