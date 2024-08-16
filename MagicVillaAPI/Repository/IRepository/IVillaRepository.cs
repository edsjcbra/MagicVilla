using System.Linq.Expressions;
using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository;

public interface IVillaRepository
{
    Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null);
    Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null);
    Task CreateAsync(Villa entity);
    Task UpdateAsync(Villa entity);
    Task RemoveAsync(Villa entity);
    Task SaveAsync();
}