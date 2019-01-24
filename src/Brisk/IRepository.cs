using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brisk
{
    public interface IRepository<T>
    {
        void Create(T taskList);
        T GetById(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Update(T item);
        void Delete(Guid id);
    }
}