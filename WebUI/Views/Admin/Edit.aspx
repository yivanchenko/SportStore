<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<DomainModel.Entities.Product>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
	Admin: Edit <%= Model.Name %>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit <%= Model.Name %></h2>
    <% using (Html.BeginForm("Edit", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
        <%= Html.HiddenFor(model => model.ProductId)%>
        <%= Html.HiddenFor(model => model.CreateDate)%>        
        <p>
            Name: <%= Html.TextBoxFor(model => model.Name)%>
            <div><%: Html.ValidationMessageFor(model => model.Name)%></div>
        </p>
        <p>
            Description: <%= Html.TextBoxFor(model => model.Description)%>
            <div><%: Html.ValidationMessageFor(model => model.Description)%></div>
        </p>
        <p>
            Price: <%= Html.TextBoxFor(model => model.Price)%>
            <div><%: Html.ValidationMessageFor(model => model.Price)%></div>
        </p>         
        <p>
            Category: <%= Html.TextBoxFor(model => model.Category)%>
            <div><%: Html.ValidationMessageFor(model => model.Category)%></div>
        </p>
        <p>
            Image:
            <% if (Model.ImageData != null)
               { %>
                <img alt="Product picture." src="<%= Url.Action("GetImage", "Products", new { Model.ProductId }) %>" />
            <% }
               else
               { %> None. <% } %>
            <div>Upload new image: <input type="file" name="productImage" /></div>
        </p>           
        <p>
            <input type="submit" value="Save" />
        </p>
    <% } %>
    <div>
        <%: Html.ActionLink("Cancel and return to List", "Index") %>
    </div>
</asp:Content>

