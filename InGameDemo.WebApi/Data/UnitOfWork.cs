using InGameDemo.WebApi.Data.Interfaces;

namespace InGameDemo.WebApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public InGameDemoContext Context { get; }

        public UnitOfWork(InGameDemoContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
