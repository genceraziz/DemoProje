using InGameDemo.WebApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InGameDemo.WebApi.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private InGameDemoContext _context;

        public GenericRepository(InGameDemoContext context)
        {
            _context = context;
        }

        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var result = _context.Set<T>().Where(i => true);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.ToListAsync();
        }

        public virtual async Task<List<T>> SearchBy(Expression<Func<T, bool>> searchBy)
        {
            var result = _context.Set<T>().Where(searchBy);

            return await result.ToListAsync();
        }

        public virtual async Task<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var result = _context.Set<T>().Where(predicate);

            return await result.FirstOrDefaultAsync();
        }

        public virtual async Task<bool> Update(T entity)
        {
            try
            {
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> Delete(Expression<Func<T, bool>> identity)
        {
            var results = _context.Set<T>().Where(identity);

            try
            {
                _context.Set<T>().RemoveRange(results);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await Task.FromResult(true);
        }

        public virtual async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
