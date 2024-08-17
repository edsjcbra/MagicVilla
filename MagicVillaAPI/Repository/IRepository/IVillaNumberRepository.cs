using System.Linq.Expressions;
using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}