using System;

namespace InGameDemo.WebApi.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationContext Context { get; }
        void Commit();
    }
}
