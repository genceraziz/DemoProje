using System;
using System.Threading.Tasks;

namespace InGameDemo.WebApi.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        InGameDemoContext Context { get; }

        Task<bool> Save();

        IGenericRepository<Categories> CategoryRepository { get; }
    }
}
