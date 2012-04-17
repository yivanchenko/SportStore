<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/WebUI.Master" Inherits="System.Web.Mvc.ViewPage<DomainModel.Entities.Cart>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    SportStore: Your Cart
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Your Cart</h2>
    <table width="90%" align="center">
        <thead>
            <tr>
                <th align="center">Quanity</th>
                <th align="left">Item</th>
                <th align="right">Price</th>
                <th align="right">Subtotal</th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var line in Model.Lines) %>
            <% { %>
                <tr>
                    <td align="center"><%= line.Quanity %></td>
                    <td align="left"><%= line.Product.Name %></td>
                    <td align="right"><%= line.Product.Price.ToString("c") %></td>
                    <td align="right"><%= (line.Quanity * line.Product.Price).ToString("c")%></td>
                    <td>
                        <% using(Html.BeginForm("RemoveFromCart", "Cart")) { %>
                            <%= Html.Hidden("productID", line.Product.ProductId)%>
                            <%= Html.Hidden("returnUrl", ViewData["returnUrl"]) %>
                            <input type="submit" value="Remove" />
                        <% } %>
                    </td>
                </tr>
            <% } %>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3" align="right">Total:</td>
                <td align="right">
                    <%=Model.ComputeTotalValue().ToString("c") %>
                </td>
            </tr>
        </tfoot>
    </table>
    <p align="center" class="actionButtons">
        <a href="<%= Html.Encode(ViewData["returnUrl"]) %>">Continue shopping</a>
        <%= Html.ActionLink("Check out now", "CheckOut") %>
    </p>
</asp:Content>
