using BadMelon.Data.DTOs;
using BadMelon.Data.Entities;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface IUserService
    {
        Task<LoginResponse> Login(Login login);

        Task<LoginResponse> Login(Guid code);

        Task<User> GetUserById(string id);

        User GetLoggedInUser();

        Task<bool> IsLoggedIn();

        Task Register(Registration registration);

        Task Verify(Guid verificationId);

        Task Reset(PasswordReset reset);
    }
}