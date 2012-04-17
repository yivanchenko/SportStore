using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel.Entities;

namespace Tests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Cart_Starts_Empty()
        {
            Cart cart = new Cart();
            Assert.AreEqual(0, cart.Lines.Count);
            Assert.AreEqual(0, cart.ComputeTotalValue());
        }

        [TestMethod]
        public void Can_Add_Items_To_Cart()
        {
            Product p1 = new Product() { ProductId = 1 };
            Product p2 = new Product() { ProductId = 2 };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 10);

            Assert.AreEqual(2, cart.Lines.Count, "Wrong number of lines in cart");

            var p1line = cart.Lines.Where(l => l.Product.ProductId == 1).First();
            var p2line = cart.Lines.Where(l => l.Product.ProductId == 2).First();

            Assert.AreEqual(3, p1line.Quanity);
            Assert.AreEqual(10, p2line.Quanity);
        }

        [TestMethod]
        public void Can_Be_Cleared()
        {
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            Assert.AreEqual(1, cart.Lines.Count);
            cart.Clear();
            Assert.AreEqual(0, cart.Lines.Count);
        }

        [TestMethod]
        public void Calculate_Total_Value_Correctly()
        {
            Cart cart = new Cart();
            cart.AddItem(new Product() { ProductId = 1, Price = 5}, 10);
            cart.AddItem(new Product() { ProductId = 2, Price = 2.1M }, 3);
            cart.AddItem(new Product() { ProductId = 3, Price = 1000 }, 1);

            Assert.AreEqual(1056.3M, cart.ComputeTotalValue());
        }

        [TestMethod]
        public void Remove_Line()
        {
            Cart cart = new Cart();
            Product product = new Product() { ProductId = 1, Price = 5 };
            cart.AddItem(product, 10);
            Assert.AreEqual(1, cart.Lines.Count);
            cart.RemoveLine(product);
            Assert.AreEqual(0, cart.Lines.Count);
        }

        [TestMethod]
        public void Cart_Shipping_Details_Starts_Empty()
        {
            Cart cart = new Cart();
            ShippingDetails details = cart.ShippingDetails;
            Assert.IsNull(details.Name);
            Assert.IsNull(details.Line1);
            Assert.IsNull(details.Line2);
            Assert.IsNull(details.Line3);
            Assert.IsNull(details.City);
            Assert.IsNull(details.State);
            Assert.IsNull(details.Country);
            Assert.IsNull(details.Zip);
        }

        [TestMethod]
        public void Cart_Not_GiftWrapped_by_Default()
        {
            Cart cart = new Cart();
            Assert.IsFalse(cart.ShippingDetails.GiftWrap);
        }
    }
}
