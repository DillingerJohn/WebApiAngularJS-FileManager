using FileManager.Core.Data.Repositories;
using FileManager.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FileManager.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        
        void BeginTransaction();
        
        int Commit();
        
        Task<int> CommitAsync();

        void Rollback();

        void Dispose(bool disposing);

    }
}
