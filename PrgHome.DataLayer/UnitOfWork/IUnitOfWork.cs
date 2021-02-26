using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PrgHome.DataLayer.Repository;

namespace PrgHome.DataLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        PrgHomeContext _context { get; }
        IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity:class;
        Task Commit();
    }
}
