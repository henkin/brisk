using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace TemplateLibrary.Repository
{
    public class MongoBase
    {
        private MongoClient _client;
        private MongoServer _mongoServer;
        protected MongoDatabase _mongoDatabase;

        public MongoBase() : this("") {}
        public MongoBase(string databaseNameSuffix)
        {
            const string connectionString = "mongodb://localhost";
            _client = new MongoClient(connectionString);
            _mongoServer = _client.GetServer();

            var dbString = ConfigurationManager.AppSettings["Database"];
            _mongoDatabase = _mongoServer.GetDatabase(dbString + databaseNameSuffix);
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