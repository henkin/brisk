using System;
using System.Threading;
using NUnit.Framework;


namespace Brisk.Tests
{
    [TestFixture]
    public class EventPublisherTests
    {
        [Test]
        public void CreateWebAndWorkerNodes()
        {
            Host worker = Host.Create();
            Host web = Host.Create();
            worker.Init();
            web.Init();

            object recievedEvent = null;
            worker.Eventer.OnEvent((ev) => { recievedEvent = ev; });
            
            web.Commander.Create(new TestEntity(DateTime.Now));
            
            Thread.Sleep(500);
            Assert.IsNotNull(recievedEvent, "Didn't get event in time");
            //Assert.AreEqual(web.Repository<TestEntity>.Count(), 1);
        }
    }

    public class TestEntity
    {
        public TestEntity(DateTime now)
        {
            
        }
    }
}
