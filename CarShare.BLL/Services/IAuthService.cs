using CarShare.BLL.Dtos;
using CarShare.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegisterDto dto);
        Task<string> LoginAsync(UserLoginDto dto);
        Task LogoutAsync(); // لو هتستخدمي Token-based ممكن يكون dummy
        string GenerateJwtToken(User user);

    }

}
