<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DomainModel.Entities.Product>" %>
<div class="item">
    <% if (Model.ImageData != null)
       { %>
        <div style="float:left; margin-right:20px">
            <img alt="Product picture." 
                 src="<%= Url.Action("GetImage", "Products", new { Model.ProductId }) %>" 
                 height="70" width="70" />
        </div>        
    <% } %>
    <h3><%= Model.Name%></h3>
    <%= Model.Description %>
    <% using(Html.BeginForm("AddToCart", "Cart")) { %>
        <%= Html.Hidden("productID") %>
        <%= Html.Hidden("returnUrl", ViewContext.HttpContext.Request.Url.PathAndQuery) %>
        <input type="submit" value="+ Add to cart" />    
    <% } %>
    <h4><%= Model.Price.ToString("c")%></h4>
</div>
