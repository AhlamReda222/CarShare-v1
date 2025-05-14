using CarShare.DAL.Models;
using CarShare.DAL.Repository;
using System.Threading.Tasks;
using CarShare.DAL.Pepository.UserRepository;
namespace CarShare.DAL.Pepository.UserRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);


    }
}
