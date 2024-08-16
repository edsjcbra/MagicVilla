using System.Linq.Expressions;
using MagicVillaAPI.Data;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Repository;

public class VillaRepository : IVillaRepository
{
    private readonly AppDbContext _db;

    public VillaRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
    {
        IQueryable<Villa> query = _db.Villas;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.ToListAsync();
    }

    public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null)
    {
        IQueryable<Villa> query = _db.Villas;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Villa entity)
    {
        await _db.Villas.AddAsync(entity);
        await SaveAsync();
    }

    public async Task UpdateAsync(Villa entity)
    {
        _db.Villas.Update(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(Villa entity)
    {
        _db.Villas.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}