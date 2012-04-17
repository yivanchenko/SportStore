namespace WebUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DomainModel.Abstract;
    using DomainModel.Entities;
    using DomainModel.Services;

    public class CartController : Controller
    {
        private IProductsRepository productRepository;
        private IOrderSubmitter orderSubmitter;

        public CartController(IProductsRepository productsRepository, IOrderSubmitter orderSubmitter)
        {
            this.productRepository = productsRepository;
            this.orderSubmitter = orderSubmitter;
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productID, string returnUrl)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductId == productID);
            cart.AddItem(product, 1);
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productID, string returnUrl)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductId == productID);
            cart.RemoveLine(product);
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            ViewData["CurrentCategory"] = "Cart";
            return View(cart);
        }

        public ViewResult Summary(Cart cart)
        {
            return View(cart);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult CheckOut(Cart cart)
        {
            return View(cart.ShippingDetails);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ViewResult CheckOut(Cart cart, FormCollection formData)
        {
            if (cart.Lines.Count == 0)
            {
                ModelState.AddModelError("Cart", "Sorry, your cart is empty!");
                return View();
            }

            if (TryUpdateModel(cart.ShippingDetails, formData.ToValueProvider()))
            {
                orderSubmitter.SubmitOrder(cart);
                cart.Clear();
                return View("Completed");
            }
            else return View();
        }
    }
}
