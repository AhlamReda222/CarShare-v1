using CarShare.BLL.Dtos;
using CarShare.DAL.Models;
using CarShare.DAL.Pepository.UserRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarShare.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // التسجيل باستخدام الـ DTO
        public async Task<string> RegisterAsync(UserRegisterDto dto)
        {
            // التحقق من إذا كان اسم المستخدم موجود بالفعل
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new Exception("Username already exists");

            // تحويل الـ DTO إلى كائن User
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                UserType = dto.UserType,
                IsApproved = dto.UserType != UserType.Owner // إذا كان Owner, يظل في حالة انتظار موافقة Admin

            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // توليد JWT Token بعد التسجيل
            return GenerateJwtToken(user);
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email); // من DAL

            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password");

            if (user.UserType == UserType.Owner && !user.IsApproved)
                throw new Exception("Owner account not approved by admin yet.");

            return GenerateJwtToken(user);
        }



        public string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("UserType", ((int)user.UserType).ToString()),
            new Claim("IsApproved", user.IsApproved.ToString())
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        // تشفير الباسورد
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);  // تأكد من إضافة مكتبة BCrypt
        }

        // التحقق من الباسورد
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);  // استخدام BCrypt للتحقق من الباسورد
        }

        public Task LogoutAsync()
        {
            // هذا الـ method قد يكون فارغًا لو كنتِ تستخدمين الـ JWT
            // لأن الـ JWT لا يحتاج إلى الخروج من السيرفر بل ينتهي صلاحيته بعد مدة معينة
            return Task.CompletedTask;
        }
    }
}





