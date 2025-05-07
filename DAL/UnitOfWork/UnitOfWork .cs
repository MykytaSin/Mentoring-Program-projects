using DAL.Interfaces;
using DAL.Models;
using DAL.Repository;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyAppContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed = false;

        public UnitOfWork(MyAppContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IAsyncRepository<TEntity> Repository <TEntity>() where TEntity : class
        {
            if(_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IAsyncRepository<TEntity>)repository;
            }
            var newRepository = new GenericAsyncRepository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), newRepository);
            return newRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        protected virtual async Task DisposeAsyncCore()
        {
            if (!_disposed)
            {
                _disposed = true;
                await _context.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

    }
}
