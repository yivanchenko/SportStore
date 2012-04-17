using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Web.Mvc;
using WebUI.HtmlHelpers;

namespace Tests
{
    [TestClass]
    public class PageingHelperTest
    {
        [TestMethod]
        public void PageLinks_Method_Extends_HtmlHelper()
        {
            HtmlHelper html = null;
            html.PageLinks(0, 0, null);
        }

        [TestMethod]
        public void PageLinks_Produces_Anchor_Tags()
        {
            string links = ((HtmlHelper)null).PageLinks(2, 3, i => "Page" + i);
            Assert.AreEqual(@"<a href=""Page1"">1</a>
<a class=""selected"" href=""Page2"">2</a>
<a href=""Page3"">3</a>
", links);
        }
    }
}
