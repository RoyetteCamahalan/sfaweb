<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="SimpleFFO._404" %>
    <%@ Import Namespace="SimpleFFO.Model" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title> <%= AppModels.Company.appname %> | 404</title>
    
    <webopt:BundleReference runat="server" Path="~/Content/maincss" />
</head>
<body class="hold-transition login-page">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        
    <!-- Main content -->
    <section class="content">
      <div class="error-page">
        <h2 class="headline text-warning"> 404</h2>

        <div class="error-content">
          <h3><i class="fas fa-exclamation-triangle text-warning"></i> Oops! Access Denied/Page not found.</h3>

          <p>
            We could not find the page you were looking for or you dont have access to this page.
            Meanwhile, you may <a href='<%= GetRouteUrl(AppModels.Routes.dashboard, null) %>'>return to dashboard</a>.
          </p>
        </div>
        <!-- /.error-content -->
      </div>
      <!-- /.error-page -->
    </section>
    <!-- /.content -->
    </form>
    <!-- /.login-box -->

    <!-- jQuery -->
    <%: Scripts.Render("~/bundles/jquery") %>
    <%: Scripts.Render("~/bundles/adminlte") %>
</body>
</html>
