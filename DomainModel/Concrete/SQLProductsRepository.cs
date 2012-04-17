using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DomainModel.Abstract;
using DomainModel.Entities;
using System.Data.Linq;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Concrete
{
    public class SQLProductsRepository : IProductsRepository
    {
        private Table<Product> productsTable;

        public SQLProductsRepository(ConnectionStringSettings connectionStringSettings)
        {
            this.productsTable = new DataContext(connectionStringSettings.ConnectionString).GetTable<Product>();
        }

        public IQueryable<Product> Products
        {
            get 
            {
                return this.productsTable; 
            }
        }
        
        public void SaveProduct(Product product)
        {
            ValidationContext validationContext = new ValidationContext(product, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (Validator.TryValidateObject(product, validationContext, validationResults))
            {
                if (product.ProductId == 0)
                {
                    this.productsTable.InsertOnSubmit(product);
                }
                else
                {
                    this.productsTable.Attach(product);
                    this.productsTable.Context.Refresh(RefreshMode.KeepCurrentValues, product);
                }

                this.productsTable.Context.SubmitChanges();
            }
            else
            {
                throw new InvalidOperationException("The object is invalid");
            }
        }

        public void DeleteProduct(Product product)
        {
            this.productsTable.DeleteOnSubmit(product);
            this.productsTable.Context.SubmitChanges();
        }
    }
}
