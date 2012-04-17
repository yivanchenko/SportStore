using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainModel.Abstract;
using DomainModel.Entities;

namespace WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private IProductsRepository productRepository;

        public int PageSize = 4;

        public ProductsController(IProductsRepository productRepository)
        {
            this.productRepository = productRepository;
        }        

        public ViewResult List(string category, int page)
        {
            var productsByCategory = string.IsNullOrEmpty(category) ? productRepository.Products :
                productRepository.Products.Where(product => product.Category == category);

            int numProducts = productsByCategory.Count();
            ViewData["TotalPages"] = (int)Math.Ceiling((double)numProducts / PageSize);
            ViewData["CurrentPAge"] = page;
            ViewData["CurrentCategory"] = category;
            return View(productsByCategory
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList());
        }

        public FileContentResult GetImage(int productId)
        {
            Product product = (from p in productRepository.Products
                               where p.ProductId == productId
                               select p).First();
            return File(product.ImageData, product.ImageMimeType);
        }
    }
}
