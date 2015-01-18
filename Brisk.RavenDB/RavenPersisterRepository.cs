using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Brisk.Persistence;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace Brisk.RavenDB
{
    public class RavenPersisterRepository : IPersister, IService, IRepository
    {
        private static EmbeddableDocumentStore _documentStore;
        //public void Handle(PersistenceEvent args)
        //{
        //    //Type entityType = Type.GetType(args.EntityType);
        //    //var set = this.Set(entityType);
        //    //var entity = JsonConvert.DeserializeObject(args.SerializedEntity, entityType);

        //    switch (args.PersistenceAction)
        //    {
        //        case PersistenceAction.Create: Add(args.Entity);
        //            break;
        //        case PersistenceAction.Update: Update(args.Entity);
        //            break;
        //        case PersistenceAction.Delete: Delete(args.Entity);
        //            break;
        //    }
        //}

        static RavenPersisterRepository()
        {
            //if (Directory.Exists("Data"))
            //    Directory.Delete("Data");
            _documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data4", EnlistInDistributedTransactions = false
                //UseEmbeddedHttpServer = true
            };
            _documentStore.Initialize(); // initializes document store, by connecting to server and downloading various configurations
            //DeleteFiles(_documentStore);
        }

        //private static void DeleteFiles(IDocumentStore documentStore)
        //{
        //    var staleIndexesWaitAction = new Action(
        //        delegate
        //        {
        //            while (documentStore.DatabaseCommands.GetStatistics().StaleIndexes.Length != 0)
        //            {
        //                Thread.Sleep(10);
        //            }
        //        });
        //    staleIndexesWaitAction.Invoke();
        //    documentStore.DatabaseCommands.DeleteByIndex("Auto/AllDocs", new IndexQuery());
        //    staleIndexesWaitAction.Invoke();
        //}

        public void Add<T>(T entity) where T : Entity
        {
            // CREATE
            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;

            using (var session = _documentStore.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();

            }
        }

        public void Update<T>(T entity) where T : Entity
        {
            // UPDATE
            entity.UpdatedAt = DateTime.UtcNow;

            using (var session = _documentStore.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }
        }

        public void Delete<T>(T entity) where T : Entity
        {
            using (var session = _documentStore.OpenSession())
            {
                var reloaded = session.Load<T>(entity.Id);
                session.Delete(reloaded);
                session.SaveChanges();
            }
        }

        public T GetByID<T>(Guid id) where T : class, IIdentifiable
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                var entity = session.Load<T>(id);//.Single(t => t.ID == id);
                return entity;
            }
        }

        public IQueryable<T> Repo<T>() where T : class, IIdentifiable
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                var entity = session
                    .Query<T>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite());
                return entity;
            }
        }

        public IQueryable<T> FindByOwnerID<T>(Guid id) where T : class, IIdentifiable, IOwnable
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //public void Clear<T>()
        //{
        //    using (IDocumentSession session = _documentStore.OpenSession())
        //    {
        //        session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName",
        //            new IndexQuery {Query = "Tag:" + "TestItem"}
        //            );
        //    }
        //}
    }
}
