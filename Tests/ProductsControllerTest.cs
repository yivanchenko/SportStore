using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DomainModel.Abstract;
using DomainModel.Entities;
using WebUI.Controllers;

namespace Tests
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public void List_Includes_All_Products_When_Category_Is_Null()
        {
            // Mock repository
            IProductsRepository repository = MockProductsRepository(
                new Product { Name = "Artemius", Category = "Greek" },
                new Product { Name = "Neptune", Category = "Roman" }
                );

            // Controller
            ProductsController controller = new ProductsController(repository);
            controller.PageSize = 10;

            // List without filter
            var result = controller.List(null, 1);
            Assert.IsNotNull(result, "Didn't render view");

            var products = (IList<Product>)result.ViewData.Model;
            Assert.AreEqual(2, products.Count, "Got wrong numbers of categories");
            Assert.AreEqual("Artemius", products[0].Name);
            Assert.AreEqual("Neptune", products[1].Name);
        }

        [TestMethod]
        public void List_Filters_By_Category_When_Requested()
        {
            // Mock repository
            IProductsRepository repository = MockProductsRepository(
                new Product { Name = "Snowball", Category = "Cats" },
                new Product { Name = "Rex", Category = "Dogs" },
                new Product { Name = "Catface", Category = "Cats" },
                new Product { Name = "Woofer", Category = "Dogs" },
                new Product { Name = "Chomper", Category = "Dogs" }
                );

            // Controller
            ProductsController controller = new ProductsController(repository);
            controller.PageSize = 10;

            // List without filter
            var result = controller.List("Dogs", 1);
            Assert.AreNotEqual(result, "Didn't render view");

            var products = (IList<Product>)result.ViewData.Model;
            Assert.AreEqual(3, products.Count);

            Assert.AreEqual("Rex", products[0].Name);
            Assert.AreEqual("Woofer", products[1].Name);
            Assert.AreEqual("Chomper", products[2].Name);
            Assert.AreEqual("Dogs", result.ViewData["CurrentCategory"]);
        }

        [TestMethod]
        public void List_Presence_Correct_list_of_Products()
        {
            IProductsRepository repository = MockProductsRepository(
                new Product { Name = "P1" }, new Product { Name = "P2" },
                new Product { Name = "P3" }, new Product { Name = "P4" },
                new Product { Name = "P5" }
                );

            ProductsController controller = new ProductsController(repository);
            controller.PageSize = 3;
            var result = controller.List(null, 2);

            Assert.IsNotNull(result, "Didn't render view");

            var products = result.ViewData.Model as IList<Product>;
            Assert.AreEqual(2, products.Count, "Got wrong number of products");
            Assert.AreEqual(2, (int)result.ViewData["CurrentPage"], "Wrong page number");
            Assert.AreEqual(2, (int)result.ViewData["TotalPages"], "Wrong page count");
            Assert.AreEqual("P4", products[0].Name);
            Assert.AreEqual("P5", products[1].Name);
        }

        private IProductsRepository MockProductsRepository(params Product[] products)
        {
            var _mockProductsRepos = new Moq.Mock<IProductsRepository>();
            _mockProductsRepos.Setup(x => x.Products).Returns(products.AsQueryable());
            return _mockProductsRepos.Object;
        }
    }
}
