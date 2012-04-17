namespace WebUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DomainModel.Abstract;
    using DomainModel.Entities;

    [Authorize]
    public class AdminController : Controller
    {
       private IProductsRepository productRepository;

       public AdminController(IProductsRepository productRepository)
       {
           this.productRepository = productRepository;
       }

       public ViewResult Index()
       {
           return View(this.productRepository.Products.ToList());
       }

       [AcceptVerbs(HttpVerbs.Get)]
       public ViewResult Edit(int productId)
       {
           var product = this.productRepository.Products.First(x => x.ProductId == productId);
           return View(product);
       }

       [AcceptVerbs(HttpVerbs.Post)]
       public ActionResult Edit(Product product, HttpPostedFileBase productImage)
       {
           if (this.ModelState.IsValid)
           {
               if (productImage != null)
               {
                   product.ImageMimeType = productImage.ContentType;
                   product.ImageData = new byte[productImage.ContentLength];
                   productImage.InputStream.Read(product.ImageData, 0, productImage.ContentLength);
               }

               if (product.ProductId == 0)
               {
                   product.CreateDate = DateTime.Now;
               }

               productRepository.SaveProduct(product);
               TempData["message"] = string.Format("{0} has been saved.", product.Name);
               return RedirectToAction("Index");
           }
           else
           {
               return View(product);
           }
       }

       public ViewResult Create()
       {
           return View("Edit", new Product());
       }

       public RedirectToRouteResult Delete(int productId)
       {
           Product product = (from p in this.productRepository.Products where p.ProductId == productId select p).First();
           this.productRepository.DeleteProduct(product);
           TempData["message"] = string.Format("Product {0} has been deleted.", product.Name);
           return RedirectToAction("Index");
       }
    }
}
