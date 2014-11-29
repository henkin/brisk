using NUnit.Framework;

namespace Brisk.Tests
{
    [TestFixture]
    public class ApplicationFixture
    {
        private Application _app;
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
        public void AppDeployed_StartsUp_ShowsPrompt_TakesCommand()
        {
            // one app

            // test all three layers. 
        }
    }

    internal class Application
    {
        public static Application Create()
        {

            return null;
        }
    }
}