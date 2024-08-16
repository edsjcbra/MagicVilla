using System.Linq.Expressions;
using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}