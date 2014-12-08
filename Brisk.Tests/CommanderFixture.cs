﻿using System;
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
        public void Create_Default_Returns_Successful_CommandResult()
        {
            var app = Application.Create();

            var result = app.Commander.Create(new TestItem());
            var actualResult = result.Result;
            
            Assert.That(actualResult, Is.InstanceOf<CommandResult<Create<TestItem>>>());
            Assert.That(actualResult.IsSuccess, "Unsuccessful");
        }

        [Test]
        public void Create_Default_Raises_Failed_Event_When_Failed()
        {

        }
    }


    public class TestItem : Entity
    {
    }

    public class TestItemService : EntityService<TestItem>
    {
        
    }
}