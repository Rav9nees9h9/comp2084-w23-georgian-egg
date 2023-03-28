using GeorgianEgg.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GeorgianEggTests
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange - setting up the data for the test
            controller = new HomeController();
	    }

        [TestMethod]
        public void IndexIsNotNull()
        {
            // Act - actually doing the action that we're testing
            var result = controller.Index();

            // Assert - checking if the result is what we expect
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexLoadsIndexView()
        { 
            var result = (ViewResult) controller.Index();

            Assert.AreEqual("Index", result.ViewName);
	    }

        [TestMethod]
        public void IndexViewDataHasMessage()
        {
            var result = (ViewResult)controller.Index();

            Assert.AreEqual("Hey class!", result.ViewData["Message"]);
	    }


        // TDD (Test-Driven Development Example)

        [TestMethod]
        public void HelloWorldIsNotNull()
        {
            var result = controller.HelloWorld();

            Assert.IsNotNull(result);
	    }

        [TestMethod]
        public void HelloWorldReturnsHelloWorld()
        {
            var result = (ViewResult)controller.HelloWorld();

            Assert.AreEqual("HelloWorld", result.ViewName);
	    }
    }
}