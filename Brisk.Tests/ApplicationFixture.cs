using System.CodeDom;
using NUnit.Framework;

namespace Brisk.Tests
{
    [TestFixture]
    public class ApplicationFixture
    {

        private IApplication _app;
        public ApplicationFixture()
        {
            
             _app = Application.Create();

        }

        /// <summary>
        /// Creates a console app.
        /// Creates a web app.
        /// Creates an api app.
        /// 
        /// get prompt. Issue command, get response. 
        /// </summary>
        [Test]
        public void AppDeployed_Create_TestItem_GetSuccessResponse_QueryForItem_LookForEvents()
        {
            //_app.Start();

            var testItem = new TestItem();
            var command = _app.Commander.Create(testItem);

            
        }
    }
}