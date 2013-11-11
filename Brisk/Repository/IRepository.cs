using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateLibrary
{
    public interface IRepository : IDisposable
    {
        T GetByID<T>(Guid id) where T : class, IIdentifiable;
        IQueryable<T> FindByOwnerID<T>(Guid id) where T : class, IIdentifiable, IOwnable;
        IQueryable<T> Repo<T>() where T : class, IIdentifiable;
    }
}
