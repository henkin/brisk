using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Todos.Web;

namespace Brisk.Web.Tests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                //.UseContentRoot(GetContentRootPath())
                .UseEnvironment("Development")
                .UseStartup<Startup>();  
            
            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}