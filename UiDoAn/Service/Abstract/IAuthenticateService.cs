using Microsoft.AspNetCore.Mvc;
using UiDoAn.Models.Domain;
using UiDoAn.Models.DTO;
using UiDoAn.Models.ViewModel;

namespace UiDoAn.Service.Abstract
{
    public interface IAuthenticateService
    {
        Task<Result<List<string>>> GetAvailableRolesAsync();
        Task<Result<UserProfileVm>> Login(LoginModel model);
        Task<Result<string>> Logout();
        Task<Result<string>> Register(RegistrationModel model);
        Task<Result<UserProfileVm>> GetCurrentUserInfoAsync();
    }
}
