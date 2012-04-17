using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using System.Web.Routing;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductsRepository _productRepository;

        public NavController(IProductsRepository productsRepository)
        {
            this._productRepository = productsRepository;
        }

        public ViewResult Menu(string highlightedCategory)
        {
            return View(new[] 
            { 
                new CategoryLink(null) 
                { 
                    IsSelected = (highlightedCategory == null) 
                } 
            }
            .Concat(_productRepository.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(c => new CategoryLink(c) { IsSelected = string.Equals(c, highlightedCategory) })
                    ).ToList<NavLink>()
            );
        }
    }

    public class NavLink
    {
        public string Text { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public bool IsSelected { get; set; }
    }

    public class CategoryLink : NavLink
    {
        public CategoryLink(string category)
        {
            IsSelected = false;
            Text = category ?? "Home";
            RouteValues = new RouteValueDictionary(new
            {
                controller = "Products",
                action = "List",
                category = category,
                page = 1
            });
        }
    }
}
