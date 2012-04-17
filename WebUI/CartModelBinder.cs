namespace WebUI
{
    using System;
    using System.Web.Mvc;
    using DomainModel.Entities;

    public class CartModelBinder : IModelBinder
    {
        private string cartSessionKey = "_cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
            {
                throw new InvalidOperationException("Couldn't refresh instances");
            }

            Cart cart = (Cart)controllerContext.HttpContext.Session[cartSessionKey];
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[cartSessionKey] = cart;
            }

            return cart;
        }
    }
}