using System;

namespace InGameDemo.WebApi.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        InGameDemoContext Context { get; }
        void Commit();
    }
}
