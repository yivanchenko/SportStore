using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class FakeProductsRepository : IProductsRepository
    {
        private static IQueryable<Product> _fakeProducts = new List<Product>
        {
            new Product() { Name="Football", Price=25 },
            new Product() { Name="Surf board", Price=40 },
            new Product() { Name="Running shoes", Price=100 }
        }.AsQueryable();
        

        public IQueryable<Product> Products
        {
            get
            {
                return _fakeProducts;
            }
        }

        public void SaveProduct(Product Product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
