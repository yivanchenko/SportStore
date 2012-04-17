namespace Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DomainModel.Abstract;
    using DomainModel.Entities;
    using System.Web.Mvc;
    using WebUI.Controllers;
    using Moq;
    using DomainModel.Services;

    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void Can_Add_Product_To_Cart()
        {
            var products = new[] {
                new Product { ProductId = 14, Name = "A" },
                new Product { ProductId = 27, Name = "B" }
            };

            var mockProductsRepo = new Moq.Mock<IProductsRepository>();
            mockProductsRepo.Setup(x => x.Products).Returns(products.AsQueryable());

            Cart cart = new Cart();
            var controller = new CartController(mockProductsRepo.Object, null);

            RedirectToRouteResult result =
                controller.AddToCart(cart, 27, "someReturnUrl");

            Assert.AreEqual(1, cart.Lines.Count);
            Assert.AreEqual("B", cart.Lines[0].Product.Name);
            Assert.AreEqual(1, cart.Lines[0].Quanity);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("someReturnUrl", result.RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void Can_Remove_Product_From_Cart()
        {
            var products = new[] {
                new Product { ProductId = 14, Name = "A" },
                new Product { ProductId = 27, Name = "B" }
            };

            var mockProductsRepo = new Moq.Mock<IProductsRepository>();
            mockProductsRepo.Setup(x => x.Products).Returns(products.AsQueryable());

            Cart cart = new Cart();
            var controller = new CartController(mockProductsRepo.Object, null);

            controller.AddToCart(cart, 27, "someReturnUrl");

            RedirectToRouteResult result =
                controller.RemoveFromCart(cart, 27, "someReturnUrl");

            Assert.AreEqual(0, cart.Lines.Count);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("someReturnUrl", result.RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void Index_Action_Renders_Defualt_View_With_Cart_And_ReturnUrl()
        {
            Cart cart = new Cart();
            CartController controller = new CartController(null, null);

            ViewResult result = controller.Index(cart, "myReturnUrl");

            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.AreSame(cart, result.ViewData.Model);
            Assert.AreEqual("myReturnUrl", result.ViewData["returnUrl"]);
            Assert.AreEqual("Cart", result.ViewData["CurrentCategory"]);
        }

        [TestMethod]
        public void Submitting_Order_With_No_Lines_Display_Default_View_With_Error()
        {
            CartController cartController = new CartController(null, null);
            Cart cart = new Cart();

            var result = cartController.CheckOut(cart, new FormCollection());

            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Submitting_Empty_Shipping_Details_Display_Default_View_With_Error()
        {
            CartController cartController = new CartController(null, null) 
            { 
                ControllerContext = new ControllerContext() 
            };

            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);            

            var result = cartController.CheckOut(cart, new FormCollection() { 
                { "Name", "" } 
            });

            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Valid_Order_Goes_To_Submitter_And_Displays_Completed_View()
        {
            var submitter = new Moq.Mock<IOrderSubmitter>();
            CartController cartController = new CartController(null, submitter.Object) 
            { 
                ControllerContext = new ControllerContext() 
            };

            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            var formData = new FormCollection() {
                {"Name", "Steve"}, {"Line1", "123 My Street"}, {"Line2", "My Area"}, {"Line3", string.Empty},
                {"City", "MyCity"}, {"State", "Some state"}, {"Zip", "123DFSDF"}, {"Country", "Far far away"},
                {"GiftWrap", bool.TrueString}
            };


            var result = cartController.CheckOut(cart, formData);

            Assert.AreEqual("Completed", result.ViewName);
            submitter.Verify(x => x.SubmitOrder(cart));
            Assert.AreEqual(0, cart.Lines.Count);
        }
    }
}
