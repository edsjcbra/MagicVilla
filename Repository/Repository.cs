using System.Linq.Expressions;
using MagicVilla.Api.Data;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _db;
    internal DbSet<T> DbSet;

    public Repository(AppDbContext db)
    {
        _db = db;
        this.DbSet = _db.Set<T>();
    }
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
    {
        IQueryable<T> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        DbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}