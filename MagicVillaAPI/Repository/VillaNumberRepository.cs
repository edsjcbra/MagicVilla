using System.Linq.Expressions;
using MagicVillaAPI.Data;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Repository;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private readonly AppDbContext _db;

    public VillaNumberRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }
    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _db.VillaNumbers.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}