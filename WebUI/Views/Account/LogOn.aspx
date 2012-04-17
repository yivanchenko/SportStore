<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Admin: Log In
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Log In</h2>
     <% if ((ViewData["lastLoginFailed"] as bool?).HasValue && (ViewData["lastLoginFailed"] as bool?).Value)
           { %>
            <div class="Message">Sorry, your login attempt failed. Try again.</div>
        <% } %>

    <p>Please log in to access the administrative area:</p>
    <% using (Html.BeginForm()) { %>
        <div>Login name: <%=Html.TextBox("username")%></div>
        <div>Password: <%=Html.Password("password")%></div>
        <p><input type="submit" value="Log In" /></p>        
    <% } %>
</asp:Content>
