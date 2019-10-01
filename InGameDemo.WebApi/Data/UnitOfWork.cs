using InGameDemo.WebApi.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace InGameDemo.WebApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<Categories> _categoryRepository;

        public UnitOfWork(InGameDemoContext context)
        {
            Context = context;
        }

        public InGameDemoContext Context { get; private set; }

        public IGenericRepository<Categories> CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new GenericRepository<Categories>(Context);
                }

                return _categoryRepository;
            }
        }

        public async Task<bool> Save()
        {
            try
            {
                int save = await Context.SaveChangesAsync();

                return await Task.FromResult(save > 0);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
