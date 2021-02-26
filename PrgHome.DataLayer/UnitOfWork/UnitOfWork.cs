using System.Threading.Tasks;
using PrgHome.DataLayer.Repository;

namespace PrgHome.DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public PrgHomeContext _context { get; }
        public UnitOfWork(PrgHomeContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity, PrgHomeContext>(_context);
        }
    }
}
