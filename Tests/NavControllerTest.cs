using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebUI.Controllers;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace Tests
{
    [TestClass]
    public class NavControllerTest
    {
        [TestMethod]
        public void Take_IProductsRepository_As_Contructor_Param()
        {
            new NavController((IProductsRepository)null);
        }

        [TestMethod]
        public void Produce_Home_Plus_Category_Links() 
        {
            IQueryable<Product> products = new[] 
            {
                new Product { Name = "A", Category = "Animals"},
                new Product { Name = "B", Category = "Vegetable"},
                new Product { Name = "C", Category = "Mineral"},
                new Product { Name = "D", Category = "Vegetable"},
                new Product { Name = "A", Category = "Animals"}
            }.AsQueryable();

            var mockProductsRepo = new Moq.Mock<IProductsRepository>();
            mockProductsRepo.Setup(x => x.Products).Returns(products);

            var controller = new NavController(mockProductsRepo.Object);
            var result = controller.Menu(null);
            var links = ((IEnumerable<NavLink>)result.ViewData.Model).ToList();

            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.AreEqual("Home", links[0].Text);
            Assert.AreEqual("Animals", links[1].Text);
            Assert.AreEqual("Mineral", links[2].Text);
            Assert.AreEqual("Vegetable", links[3].Text);

            foreach (var link in links)
            {
                Assert.AreEqual("Products", link.RouteValues["controller"]);
                Assert.AreEqual("List", link.RouteValues["action"]);
                Assert.AreEqual(1, link.RouteValues["page"]);

                if (links.IndexOf(link) == 0)
                {
                    Assert.IsNull(link.RouteValues["category"]);
                }
                else
                {
                    Assert.AreEqual(link.Text, link.RouteValues["category"]);
                }
            }
        }

        [TestMethod]
        public void Highlights_Current_Category()
        {
            IQueryable<Product> products = new[] {
                new Product { Name = "A", Category = "Animals"},
                new Product { Name = "B", Category = "Vegetable"}
            }.AsQueryable();

            var mockProductsRepo = new Moq.Mock<IProductsRepository>();
            mockProductsRepo.Setup(x => x.Products).Returns(products);
            var controller = new NavController(mockProductsRepo.Object);

            var result = controller.Menu("Vegetable");
            var highlightedLinks = ((IEnumerable<NavLink>)result.ViewData.Model).Where(x => x.IsSelected).ToList();

            Assert.AreEqual(1, highlightedLinks.Count);
            Assert.AreEqual("Vegetable", highlightedLinks[0].Text);
        }
    }
}
