using System;
using System.Linq;

namespace Brisk.Repository
{
    public interface IRepository : IDisposable
    {
        T GetByID<T>(Guid id) where T : class, IIdentifiable;
        IQueryable<T> FindByOwnerID<T>(Guid id) where T : class, IIdentifiable, IOwnable;
        IQueryable<T> Repo<T>() where T : class, IIdentifiable;
    }
}
