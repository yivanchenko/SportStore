<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DomainModel.Entities.Product>>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
	Admin: All products.
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>All products</h2>
    <table class="Grid">
        <tr>
            <th>ID</th>
            <th>Name</th>            
            <th class="NumericCol">Price</th>
            <th>Actions</th>
        </tr>
        <% foreach (var item in Model) { %>
            <tr>
                <td><%= item.ProductId %></td>
                <td><%= item.Name %></td>
                <td class="NumericCol"><%= item.Price.ToString("c") %></td>
                <td>
                    <%= Html.ActionLink("Edit", "Edit", new { item.ProductId }) %>
                    <%= Html.ActionLink("Delete", "Delete", new { item.ProductId })%>
                </td>               
            </tr>    
        <% } %>
    </table>
    <p>
        <%: Html.ActionLink("Add a new product", "Create") %>
    </p>
</asp:Content>

