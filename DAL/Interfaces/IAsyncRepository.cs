using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetById(int id);
        IQueryable<T> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        public IQueryable<T> GetAllAsync(Expression<Func<T,bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] icludes);
        public Task<T> GetByConditionAsync(Expression<Func<T, bool>> expression);
        public IQueryable<T> GetManyByConditionAsync(Expression<Func<T, bool>> expression);
    } 
}
