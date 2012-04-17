<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/WebUI.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DomainModel.Entities.Product>>" %>

<asp:Content ID="ListTitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Sportstore:
    <%= string.IsNullOrEmpty(ViewData["CurrentCategory"] as string) ? 
        "All products" : 
        Html.Encode(ViewData["CurrentCategory"])
    %>
</asp:Content>
<asp:Content ID="ListMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <% foreach (var product in Model)
       { %>
    <% Html.RenderPartial("ProductSummary", product); %>
    <% } %>
    <div class="pager">
        Page:
        <%= Html.PageLinks((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"],
        x => Url.Action("List", new { page = x, category = ViewData["CurrentCategory"] })) %>
    </div>
</asp:Content>
