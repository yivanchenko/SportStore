using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;


namespace WebUI.Controllers
{
    public class RssController : Controller
    {
        private IProductsRepository productRepository;

        public RssController(IProductsRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        
        public ActionResult Feed()
        {
            return new ContentResult() 
            { 
                Content = RssGenerator.GetRss(productRepository.Products, Request.Url) 
            };
        }
    }
}
