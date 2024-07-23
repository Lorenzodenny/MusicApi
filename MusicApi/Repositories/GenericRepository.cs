using Microsoft.EntityFrameworkCore;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.Model;
using System.Linq.Expressions;

namespace MusicApi.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MusicApiContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MusicApiContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Metodo per inserire dinamicamente gli include
        public async Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // Metodo PaginateAsync:
        // Questo metodo permette la paginazione degli elementi di tipo T e accetta un parametro 'include' opzionale.
        // Il parametro 'include' è una funzione che specifica una relazione da includere nel risultato della query.
        // Ad esempio, passando song => song.Album, indichiamo a Entity Framework di includere i dettagli dell'album associato a ogni canzone.
        // Se fornito, questo parametro è utilizzato per modificare la query e includere le relazioni specificate.

        // Pagination
        public async Task<PagedResult<T>> PaginateAsync(int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            // Applica ogni include passato al metodo
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }




    }
}
