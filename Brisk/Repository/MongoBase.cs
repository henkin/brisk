using System;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Brisk.Repository
{
    public class MongoBase
    {
        private MongoClient _client;
        private MongoServer _mongoServer;
        protected MongoDatabase _mongoDatabase;

        public MongoBase() : this("") {}
        public MongoBase(string databaseNameSuffix)
        {
            _client = new MongoClient(GetConnectionString());
            _mongoServer = _client.GetServer();

            var dbString = ConfigurationManager.AppSettings["Database"];
            _mongoDatabase = _mongoServer.GetDatabase(dbString + databaseNameSuffix);
        }

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