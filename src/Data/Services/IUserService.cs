using BadMelon.Data.DTOs;
using BadMelon.Data.Entities;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface IUserService
    {
        Task<bool> Login(Login login);

        Task<bool> Login(Guid code);

        User GetLoggedInUser();

        Task<bool> IsLoggedIn();

        Task Logout();

        Task Register(Registration registration);

        Task Verify(Guid verificationId);

        Task Reset(PasswordReset reset);
    }
}