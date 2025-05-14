using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarShare.DAL.Repository;
namespace CarShare.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();                                      // Get all
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);   // Get all with condition
        Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>> filter = null,
    string[] includes = null
);

        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);                                                 // Add new
        void Update(T entity);                                                   // Update
        void Delete(T entity);                                                   // Delete
        Task<bool> SaveChangesAsync();                                           // Save changes
    }
}
