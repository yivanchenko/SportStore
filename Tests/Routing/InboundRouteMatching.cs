using System;
using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI;

namespace Tests.Routing
{
    [TestClass]
    public class InboundRouteMatching
    {
        [TestMethod]
        public void ForwardSlashGoesToHomeIndex()
        {
            this.TestRoute("~/", new { controller = "Products", action = "List", page = 1 });
        }

        private Mock<HttpContextBase> MakeMockHttpContext(string url)
        {
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockHttpRequest = new Mock<HttpRequestBase>();
            var mockHttpResponse = new Mock<HttpResponseBase>();

            mockHttpContext.Setup(x => x.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Setup(x => x.Response).Returns(mockHttpResponse.Object);

            mockHttpRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);
            mockHttpResponse.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);

            return mockHttpContext;
        }

        private RouteData TestRoute(string url, object expectedResults)
        {
            RouteCollection routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            var mockHttpContext = MakeMockHttpContext(url);

            RouteData routeData = routes.GetRouteData(mockHttpContext.Object);
            Assert.IsNotNull(routeData, "NULL RouteData was returned");            

            foreach (var expectedVal in new RouteValueDictionary(expectedResults))
            {
                if (expectedVal.Value == null)
                    Assert.IsNull(routeData.Values[expectedVal.Key]);
                else
                    Assert.AreEqual(expectedVal.Value.ToString(), routeData.Values[expectedVal.Key].ToString());                
            }

            return routeData;
        }
    }
}
