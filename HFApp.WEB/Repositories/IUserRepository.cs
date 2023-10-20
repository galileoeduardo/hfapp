using Microsoft.AspNetCore.Identity;

namespace HFApp.WEB.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
