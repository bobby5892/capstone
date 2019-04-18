using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using UnitTests.WebTests;
namespace UnitTests
{
    public class WebEndpoint
    {
        public WebEndpoint()
        {

        }
        [Fact]
        public void LoginTest()
        {
            WebTest webTest = new WebTest();
            string response = webTest.Request("http://localhost:8080", "GET", "");

            Assert.True();


        }
    }
}
