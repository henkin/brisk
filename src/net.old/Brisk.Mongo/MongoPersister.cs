using System;
using System.Configuration;
using System.Linq;
using Brisk.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Brisk.Mongo
{
 
    public class MongoPersister :  IPersister, IRepository, IService
    {
        private MongoClient _client;
        private MongoServer _mongoServer;
        protected MongoDatabase _mongoDatabase;
        public MongoPersister() : this("") {}
        public MongoPersister(string databaseNameSuffix)
        {
            _client = new MongoClient(MongoPersister.GetConnectionString());
            _mongoServer = _client.GetServer();

            var dbString = ConfigurationManager.AppSettings["MongoDatabaseName"];
            _mongoDatabase = _mongoServer.GetDatabase(dbString + databaseNameSuffix);
        }

        public void Add<T>(T entity) where T : Entity
        {
            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
            GetCollection(entity.GetType()).Save(entity, WriteConcern.Unacknowledged);
        }

        public void Update<T>(T entity) where T : Entity
        {
            entity.UpdatedAt = DateTime.UtcNow;
            GetCollection(entity.GetType()).Save(entity, WriteConcern.Unacknowledged);
        }

        public void Delete<T>(T entity) where T : Entity
        {
            GetCollection(entity.GetType()).Remove(Query.EQ("_id", entity.Id), WriteConcern.Unacknowledged);
        }

        public T GetByID<T>(Guid id) where T : class, IIdentifiable
        {
            var collection = GetCollection<T>();
            var entity = collection.FindOneAs<T>(Query.EQ("_id", id));
            return entity;
        }

        public IQueryable<T> FindByOwnerID<T>(Guid ownerID) where T : class, IIdentifiable, IOwnable
        {
            var collection = GetCollection<T>();
            return collection.AsQueryable<T>().Where(t => t.OwnerID == ownerID);
        }

        public IQueryable<T> Repo<T>() where T : class, IIdentifiable
        {
            var collection = GetCollection<T>();
            return collection.AsQueryable<T>();
        }

        public void Dispose() { }

        private static string GetConnectionString()
        {
            const string connectionString = "mongodb://localhost";
            return connectionString;
        }

        protected MongoCollection<BsonDocument> GetCollection<T>() where T : class, IIdentifiable
        {
            return _mongoDatabase.GetCollection(GetCollectionNameFromEntityType(typeof(T)));
        }

        protected MongoCollection<BsonDocument> GetCollection(Type type)
        {
            return _mongoDatabase.GetCollection(GetCollectionNameFromEntityType(type));
        }

        private string GetCollectionNameFromEntityType(Type type)
        {
            var collectionType = type;
            while (collectionType.BaseType != typeof(Entity))
                collectionType = collectionType.BaseType;
            return collectionType.Name;
        }
    }
}