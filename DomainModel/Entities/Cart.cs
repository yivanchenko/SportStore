namespace DomainModel.Entities
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Cart
    {
        private List<CartLine> lines = new List<CartLine>();

        private ShippingDetails shippingDetails = new ShippingDetails();

        public IList<CartLine> Lines
        {
            get
            {
                return lines.AsReadOnly();
            }
        }

        public void AddItem(Product product, int quanity)         
        {
            CartLine productLine = lines.Find(l => l.Product.ProductId == product.ProductId);
            if (productLine == null)
            {
                lines.Add(new CartLine() { Product = product, Quanity = quanity });
            }
            else
            {
                productLine.Quanity += quanity;
            }
        }

        public decimal ComputeTotalValue() 
        { 
            return lines.Sum(l => l.Product.Price * l.Quanity); 
        }

        public void Clear() 
        {
            lines.Clear();
        }

        public void RemoveLine(Product product)
        {
            lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public ShippingDetails ShippingDetails 
        { 
            get 
            { 
                return shippingDetails; 
            } 
        }
    }

    public class CartLine 
    {
        public Product Product { get; set; }
        public int Quanity { get; set; }
    }
}
