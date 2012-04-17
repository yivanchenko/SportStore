namespace Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DomainModel.Abstract;
    using DomainModel.Entities;
    using WebUI.Controllers;
    using System.Web.Mvc;

    /// <summary>
    /// Summary description for AdminControllerTests
    /// </summary>
    [TestClass]
    public class AdminControllerTest
    {
        private Moq.Mock<IProductsRepository> mockRepos;
        
        [TestInitialize()]
        public void SetUp() 
        {
            List<Product> allProducts = new List<Product>();

            for (int i = 1; i <= 50; i++)
            {
                allProducts.Add(new Product() { ProductId = i, Name = string.Format("Product {0}", i) });
            }

            mockRepos = new Moq.Mock<IProductsRepository>();
            mockRepos.Setup(x => x.Products).Returns(allProducts.AsQueryable());            
        }

        [TestMethod]        
        public void Index_Action_List_All_Products()
        {
            AdminController controller = new AdminController(mockRepos.Object);
            ViewResult result = controller.Index();
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));

            var prodsRendered = (List<Product>)result.ViewData.Model;
            Assert.AreEqual(50, prodsRendered.Count);

            for (int i = 0; i < 50; i++)
            {
                Assert.AreEqual(string.Format("Product {0}", i + 1), prodsRendered[i].Name);
            }
        }

        [TestMethod]
        public void Index_Action_Edit_Get()
        {
            AdminController controller = new AdminController(mockRepos.Object);
            ViewResult result = controller.Edit(17);

            Product renderedProduct = (Product)result.ViewData.Model;

            Assert.AreEqual(17, renderedProduct.ProductId);
            Assert.AreEqual("Product 17", renderedProduct.Name);
        }

        [TestMethod]
        public void Edit_Action_Saves_Product_To_Repository_And_Redirect_To_Index()
        {
            AdminController controller = new AdminController(mockRepos.Object);
            Product newProduct = new Product();

            RedirectToRouteResult result = controller.Edit(newProduct, null) as RedirectToRouteResult;

            mockRepos.Verify(x => x.SaveProduct(newProduct));
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Delete_Action_Deletes_Product_From_Repository_Then_Redirect_To_Index()
        {
            AdminController controller = new AdminController(mockRepos.Object);
            Product productN24 = mockRepos.Object.Products.First(p => p.ProductId == 24);

            RedirectToRouteResult result = controller.Delete(24);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(string.Format("Product {0} has been deleted.", productN24.Name), controller.TempData["message"]);
            mockRepos.Verify(x => x.DeleteProduct(productN24));
        }
    }
}
