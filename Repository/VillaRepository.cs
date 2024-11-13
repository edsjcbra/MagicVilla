using MagicVilla.Api.Data;
using MagicVilla.Api.Models;
using MagicVilla.Api.Repository.IRepository;

namespace MagicVillaAPI.Repository;

public class VillaRepository : Repository<Villa>, IVillaRepository
{
    private readonly AppDbContext _db;

    public VillaRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }
    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _db.Villas.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}