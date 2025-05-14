using CarShare.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CarShare.DAL.Pepository;
using CarShare.DAL.Pepository.UserRepository;

namespace CarShare.DAL.Repository.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CarShareDbContext context) : base(context) { }

       public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
