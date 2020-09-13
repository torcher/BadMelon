using BadMelon.Data.DTOs;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface IUserService
    {
        Task<bool> Login(Login login);

        Task<bool> Login(Guid code);

        Task<bool> IsLoggedIn();

        Task Logout();

        Task Register(Registration registration);

        Task Verify(Guid verificationId);
    }
}