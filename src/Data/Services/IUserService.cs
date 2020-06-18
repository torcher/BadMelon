using BadMelon.Data.DTOs;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface IUserService
    {
        Task<bool> Login(Login login);

        Task<bool> IsLoggedIn();

        Task Logout();
    }
}