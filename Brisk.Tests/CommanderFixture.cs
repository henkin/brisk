using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Brisk.Events;
using NUnit.Framework;
using Brisk;
using Brisk.Mongo;

namespace Brisk.Tests
{
    [TestFixture]
    public class CommanderFixture
    {
        [Test]
        public async void Create()
        {
            var app = TestApp.Create();

            var entity = new TestItem() {Name = "testName"};
            var actualResult = app.Commander.Create(entity);

            Assert.That(actualResult, Is.InstanceOf<CommandResult<Create<TestItem>>>());
            Assert.That(actualResult.IsSuccess);

            var testItem = app.Repository.GetByID<TestItem>(entity.ID);
            Assert.That(testItem.Name, Is.EqualTo(entity.Name));
        }


        [Test]
        public async void Update()
        {
            var app = TestApp.Create();

            var testEntities = app.Repository.Repo<TestItem>().ToList();
            Assert.That(testEntities.Any());

            var latest = testEntities.Last();
            latest.Name = "Updated";
            var actualUpdateResult = app.Commander.Update(latest);

            var testItem = app.Repository.GetByID<TestItem>(latest.ID);
            Assert.That(testItem.Name, Is.EqualTo("Updated"));
        }

        [Test]
        public async void Delete()
        {
            var app = TestApp.Create();

            var testEntity = new TestItem() {Name = "testName"};
            var actualResult = app.Commander.Create(testEntity);

            var thenDelete = app.Commander.Delete(testEntity);

            Assert.That(actualResult.IsSuccess);

            var testItem = app.Repository.GetByID<TestItem>(testEntity.ID);
            Assert.That(testItem == null);
        }


        [Test]
        public void TenThousandTestItemsSerial()
        {
            var app = Application.Create();

            for (int j = 0; j < 32000; j++)
            {
                var testEntity = new TestItem() {Name = "testName" + Guid.NewGuid()};
                var actualResult = app.Commander.Create(testEntity);
                //var thenDelete = app.Commander.Delete(testEntity);
                Assert.That(actualResult.IsSuccess);
            }
        }

        [Test]
        public void TenThousandTestItemsParallel()
        {
            var app = Application.Create();

            const int n = 8;
            // Construct started tasks
            Task[] tasks = new Task[n];
            for (int i = 0; i < n; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                                                 {
                                                     for (int j = 0; j < 4000; j++)
                                                     {
                                                         var testEntity = new TestItem() { Name = "testName" + Guid.NewGuid() };
                                                         var actualResult = app.Commander.Create(testEntity);
                                                         //var thenDelete = app.Commander.Delete(testEntity);
                                                         Assert.That(actualResult.IsSuccess);
                                                     }

                                                 });

            }



            // Exceptions thrown by tasks will be propagated to the main thread 
            // while it waits for the tasks. The actual exceptions will be wrapped in AggregateException. 
            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks);

                // We should never get to this point
                Console.WriteLine("WaitAll() has not thrown exceptions. THIS WAS NOT EXPECTED.");
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\nThe following exceptions have been thrown by WaitAll(): (THIS WAS EXPECTED)");
                for (int j = 0; j < e.InnerExceptions.Count; j++)
                {
                    Console.WriteLine("\n-------------------------------------------------\n{0}",
                        e.InnerExceptions[j].ToString());
                }
            }
        }
    }

    public static class TestApp
    {
        public static IApplication Create()
        {
            return Application.Create(typeof(MongoPersister));
        }
    }

    public class TestItemService : EntityService<TestItem>
    {

    }
}