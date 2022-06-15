<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocationViewer.aspx.cs" Inherits="SimpleFFO.views.LocationViewer" %>

<%@ Import Namespace="SimpleFFO.Model" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title><%= AppModels.Company.appname %></title>

    <webopt:BundleReference runat="server" Path="~/Content/maincss" />
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


            <!-- Main content -->
            <section class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12">
                            <iframe id="urlframe" runat="server" frameborder="0" style="width: 100%; height: 700px; border: 0"></iframe>
                        </div>
                    </div>
                </div>
            </section>
            <!-- /.content -->
    </form>
    <!-- /.login-box -->

    <!-- jQuery -->
    <%: Scripts.Render("~/bundles/jquery") %>
</body>
</html>
