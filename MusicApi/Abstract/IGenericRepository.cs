﻿using MusicApi.Model;
using System.Linq.Expressions;

namespace MusicApi.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllIncludingAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        // pagination
        Task<PagedResult<T>> PaginateAsync(int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes);
    }
}
