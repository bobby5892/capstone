using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using UnitTests.WebTests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UnitTests
{
    public class WebEndpoint : Controller
    {
       public WebEndpoint()
        {
            
        }
        #region AccountController
        [Fact]
        public void LoginAvailable()
        {
            WebTest webTest = new WebTest();
            string response = webTest.Request("http://localhost:8080/Account/Login", "GET", "");
            // Now you have to know the Datatype your going into
            dynamic jsonObject = JObject.Parse(response);
            Assert.Equal((bool)jsonObject.success, true);
        }
        [Fact]
        public void LoginAsAdmin()
        {
            WebTest webTest = new WebTest();
            string response = webTest.Request("http://localhost:8080/Account/Login", "POST", "Email=Admin@example.com&Password=password&returnURL=");
            // Now you have to know the Datatype your going into
            dynamic jsonObject = JObject.Parse(response);
            Assert.Equal((bool)jsonObject.success, true);
        }
        [Fact]
        public void FailLogin()
        {
            WebTest webTest = new WebTest();
            string response = webTest.Request("http://localhost:8080/Account/Login", "POST", "Email=bob@example.com&Password=bob&returnURL=");
            // Now you have to know the Datatype your going into
            dynamic jsonObject = JObject.Parse(response);
            Assert.Equal((bool)jsonObject.success, false);
        }
        #endregion


    }
}
