using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}

