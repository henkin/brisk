using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Brisk.Web.ApiModels;
using Brisk.Models;
using Xunit;
using Xunit.Abstractions;

namespace Brisk.Web.Tests
{
    [Collection("Controller Tests")]
    public class ListControllerTests : BaseControllerTests, IClassFixture<TestServerFixture>
    {
        public ListControllerTests(TestServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            ControllerUrl = "lists/";
        }

        [Fact] public async Task Get_ReturnsAll()
        {
            await CreateTaskList();
            var allTaskLists = await Get<List<TaskListModel>>();
            allTaskLists.Should().NotBeEmpty();
            allTaskLists.ForEach(x => _output.WriteLine(x.ToString()));
        }
        
        [Fact] public async Task Get_SearchString()
        {
            var searchListName = "Searchable" + DateTime.Now.Ticks;
            await CreateTaskList(searchListName);
            var allTaskLists = await Get<List<TaskListModel>>(GenerateGetUrl(search: searchListName));
            allTaskLists.Should().NotBeEmpty();
            allTaskLists.ForEach(l => l.Name.Should().Be(searchListName));
        }
        
        [Fact] public async Task Get_Skip()
        {
            // create at least 2
            await CreateTaskList();
            await CreateTaskList();
            
            var allTaskLists = await Get<List<TaskListModel>>(GenerateGetUrl(skip: 1));
            allTaskLists.Should().NotBeEmpty();
            // how to test for skip?
            // the test initialization would be a fair amount of work.. 
        }
        
        [Fact] public async Task Get_Limit()
        {
            await CreateTaskList();
            var allTaskLists = await Get<List<TaskListModel>>(GenerateGetUrl(limit: 2));
            allTaskLists.Should().HaveCountLessOrEqualTo(2);
        }
        
        [Fact]
        public async Task Post_CreatesTaskList()
        {
            var savedTask = await CreateTaskList();
            var actual = await GetById<TaskListModel>(savedTask.Id);
            actual.Should().BeEquivalentTo(savedTask);
        }
        
        private string GenerateGetUrl(string search = null, int? skip = null, int? limit = null)
        {
            var url = "lists/";
            if (search != null) url += "?search=" + search;
            if (skip != null) url += "?skip=" + skip;
            if (limit != null) url += "?limit=" + limit;
            return url;
        }
    }
}