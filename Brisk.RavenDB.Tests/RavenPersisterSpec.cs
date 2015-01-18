using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brisk.Tests;
using NUnit.Framework;

namespace Brisk.RavenDB.Tests
{
    //[TestFixture]
    //public class RavenPersisterSpec
    //{
    //    [Test]
    //    public void Add_Item()
    //    {
    //        var persister = new RavenPersisterRepository();
    //        var expected = new TestItem() {Name = "testName"};
    //        persister.Add(expected);

    //        var actual = persister.GetByID<TestItem>(expected.ID);
    //        Assert.AreEqual(expected.Name, actual.Name);
    //    }
    //}

    //public class TestItem : Entity
    //{
    //    public string Name { get; set; }
    //}

    public class RavenPersisterSpec : PersisterSpec<RavenPersisterRepository>
    {

       
    }
}

