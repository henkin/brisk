using System;
using System.Dynamic;
using System.Reflection;
using Autofac;
using Brisk.Events;
using NUnit.Framework;
using Brisk;
namespace Brisk.Tests
{
    [TestFixture]
    public class CommanderFixture
    {
        [Test]
        public async void Create_Default_Returns_Successful_CommandResult()
        {
            var app = Application.Create();

            var entity = new TestItem() { Name = "testName" };
            var actualResult = app.Commander.Create(entity);


            Assert.That( actualResult,Is.InstanceOf<CommandResult<Create<TestItem>>>());
            Assert.That( actualResult.IsSuccess);

            var testItem = app.Repository.GetByID<TestItem>(entity.ID); 
            Assert.That(testItem.Name, Is.EqualTo(entity.Name));
        }

        [Test]
        public void Create_Default_Raises_Created_Event()
        {
            //var commander = new Commander();


            //var result = app.Commander.Create(new TestItem());


            //Assert.That(actualResult, Is.InstanceOf<CommandResult<Create<TestItem>>>());
            //Assert.That(actualResult.IsSuccess, "Unsuccessful");
        }


        [Test]
        public void Create_Default_Raises_Failed_Event_When_Failed()
        {

        }
    }


    public class TestItemService : EntityService<TestItem>
    {

    }
}