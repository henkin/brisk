using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Brisk.Models;
using Brisk.Web.ApiModels;
using Xunit;
using Xunit.Abstractions;

namespace Brisk.Web.Tests
{
    [Collection("Controller Tests")]
    public class TaskControllerTests : BaseControllerTests, IClassFixture<TestServerFixture>
    {
        public TaskControllerTests(TestServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            ControllerUrl = "tasks/";
        }

        [Fact] public async Task Get_ReturnsAll()
        {
            var allTaskLists = await Get<List<TaskModel>>();
            allTaskLists.Should().NotBeEmpty();
            allTaskLists.ForEach(x => _output.WriteLine(x.ToString()));
        }
        
        [Fact]
        public async Task Post_CreatesTaskForList()
        {
            var taskList = await CreateTaskList();
            var expected = taskList.Tasks.First();
            var actual = await GetById<TaskModel>(expected.Id);
            //actual.Should().Equals(expected, (e1, e2) => e1.Id == e2.Id);
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Put_UpdatesTask()
        {
            var taskList = await CreateTaskList();
            var expected = taskList.Tasks.First();
            expected.Completed = true;

            await Put(expected, expected.Id);
            
            var actual = await GetById<TodoTask>(expected.Id);
            actual.Completed.Should().BeTrue();
        }
    }
}