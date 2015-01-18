using System.Linq;
using System.Runtime.InteropServices;
using Brisk.Persistence;
using NUnit.Framework;

namespace Brisk.Tests
{
    [TestFixture]
    public abstract class PersisterSpec<T> where T : IPersister, IRepository, new()
    {
        [Test]
        public void CrudTest()
        {
            var persister = new T();
            var expected = new TestItem() { Name = "testName" };
            persister.Add(expected);

            var actual = persister.GetByID<TestItem>(expected.Id);
            Assert.AreEqual(expected.Name, actual.Name);

            expected.Name = "updatedName";
            persister.Update(expected);

            actual = persister.GetByID<TestItem>(expected.Id);
            Assert.AreEqual("updatedName", actual.Name);

            persister.Delete(actual);
        }

        [Test]
        public void SubclassTest()
        {
            var persister = new T();
            var expected = new TestSubclass() { Name = "testName", SomeValue = 123 };
            persister.Add(expected);

            var actual = persister.GetByID<TestSubclass>(expected.Id);
            Assert.AreEqual(expected.SomeValue, actual.SomeValue);

            persister.Delete(actual);
        }

        [Test]
        public void SearchTest()
        {
            var persister = new T();

            // delete all
            var list = persister.Repo<TestItem>().ToList();
            list.ForEach(i => persister.Delete(i));

            var first = new TestItem() { Name = "firstName" };
            var another = new TestItem() { Name = "anotherName" };
            var last = new TestItem() { Name = "lastName" };
            persister.Add(first);
            persister.Add(another);
            persister.Add(last);

            var count = persister.Repo<TestItem>().ToList().Count();
            Assert.AreEqual(3, count);

            var searchResult = persister.Repo<TestItem>().Single(i => i.Name == "anotherName");
            Assert.AreEqual(another.Id, searchResult.Id);

            searchResult = persister.Repo<TestItem>().OrderByDescending(i => i.CreatedAt).First();
            Assert.AreEqual(last.Id, searchResult.Id);
        }
    }
}