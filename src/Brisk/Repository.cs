using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Brisk.Models;
using Humanizer;
using LiteDB;

namespace Brisk
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly IDbProvider _dbProvider;
        private string _collectionName;

        public Repository(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
            _collectionName = typeof(T).Name.Pluralize().Underscore();
        }
        
        public void Create(T item) => Execute(col => col.Insert(item));
        public T GetById(Guid id) => Execute(col => col.FindOne(item => item.Id == id));
        public IEnumerable<T> GetAll() => Execute(col => col.FindAll());
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => Execute(col => col.Find(predicate));
        public void Update(T item) => Execute(col => col.Update(item));
        public void Delete(Guid id) => Execute(col => col.Delete(item => item.Id == id));

        private TRet Execute<TRet>(Func<LiteCollection<T>, TRet> func)
        {
            var col = _dbProvider.Db.GetCollection<T>(_collectionName);
            return func.Invoke(col);
        }
    }
}