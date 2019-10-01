using InGameDemo.WebApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InGameDemo.WebApi.Data
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public T Get(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? _unitOfWork.Context.Set<T>().FirstOrDefault() : _unitOfWork.Context.Set<T>().FirstOrDefault(filter);
        }

        public List<T> GetList(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? _unitOfWork.Context.Set<T>().ToList() : _unitOfWork.Context.Set<T>().Where(filter).ToList();
        }

        public void Update(T entity)
        {
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Context.Set<T>().Attach(entity);
        }
    }
}
