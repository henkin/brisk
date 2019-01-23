using System.Collections.Generic;
using Brisk;
using NUnit.Framework;

namespace TemplateLibrary.Tests
{
    [TestFixture]
    public class AppViewModelTests
    {
        [Test]
        public void Create()
        {
            var app = new App();
            app.Title = "FooFoo";
            var appViewModel = AppViewModel.Create(app);
             
        }
    }

    public class Menu
    {
        public List<MenuItem> MenuItems { get; set; }

        public Menu()
        {
            MenuItems = new List<MenuItem>();
        }
    }

    public class MenuItem
    {
        public string Link { get; set; }
        public string Text { get; set; }
    }

    public class App
    {
        public string Title { get; set; }
        public Menu Menu { get; set; }

        public App()
        {
            Menu = new Menu();
        }
    }
}