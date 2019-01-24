using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Brisk.Web.ApiModels;
using Brisk.Models;
using Xunit.Abstractions;

namespace Brisk.Web.Tests
{
    public class BaseControllerTests
    {
        protected TestServerFixture _fixture;
        protected ITestOutputHelper _output;
        protected string ControllerUrl;
        private Random _rand;

        protected BaseControllerTests(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _rand = new Random((int)DateTime.Now.Ticks);
        }
        
        protected async Task Delete(Guid id)
        {
            var response = await _fixture.Client.DeleteAsync(ControllerUrl + id);
            await GetSuccessfulResponseData<TodoTaskList>(response);
        }

        protected async Task<T> Post<T>(T item)
        {
            var stringContent = StringContent(item);
            var response = await _fixture.Client.PostAsync(ControllerUrl, stringContent);
            return await GetSuccessfulResponseData<T>(response);
        }

        protected async Task<T> Put<T>(T item, Guid id)
        {
            var stringContent = StringContent(item);
            var response = await _fixture.Client.PutAsync(ControllerUrl + id, stringContent);
            return await GetSuccessfulResponseData<T>(response);
        }

        protected async Task<T> Get<T>(string url = null)
        {
            var response = await _fixture.Client.GetAsync(url ?? ControllerUrl);
            return await GetSuccessfulResponseData<T>(response);
        }

        protected async Task<T> GetById<T>(Guid id)
        {
            var response = await _fixture.Client.GetAsync(ControllerUrl + id);
            return await GetSuccessfulResponseData<T>(response);
        }
        
        protected static async Task<T> GetSuccessfulResponseData<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<T>(responseString);
            return entity;
        }
        
        protected static StringContent StringContent<T>(T item)
        {
            var stringContent = new StringContent(
                JsonConvert.SerializeObject(item),
                Encoding.UTF8,
                "application/json");
            return stringContent;
        }

        /// <summary>
        /// This will be refactored into a separate Test Workflow component
        /// </summary>
        protected async Task<TaskListModel> CreateTaskList(string listName = null)
        {
            var random = _rand.Next(10000000);
            var taskList = new TaskListModel
            {
                Name = listName ?? "TestTaskList" + random,
                Description = "TestDescription",
                Tasks = new List<TaskModel>
                {
                    new TaskModel
                    {
                        Name = "TestTaskName" + random,
                        Completed = false
                    }
                }
            };
            var stringContent = StringContent(taskList);
            var response = await _fixture.Client.PostAsync("Lists/", stringContent);
            var savedTask = await GetSuccessfulResponseData<TaskListModel>(response);
            return savedTask;
        }
    }
}