using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InGameDemo.WebApi.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> Add(T entity);

        Task<T> GetById(int id);

        Task<List<T>> GetAll();

        Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes);

        Task<List<T>> SearchBy(Expression<Func<T, bool>> searchBy);

        Task<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<bool> Update(T entity);

        Task<bool> Delete(Expression<Func<T, bool>> identity);

        Task<bool> Delete(T entity);
    }
}
