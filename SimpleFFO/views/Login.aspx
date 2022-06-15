<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SimpleFFO.views.Login" %>

<%@ Import Namespace="SimpleFFO.Model" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title><%= AppModels.Company.appname %> | Log in</title>

    <webopt:BundleReference runat="server" Path="~/Content/maincss" />


</head>
<body class="hold-transition login-page">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <div class="login-box">
            <div class="login-logo">
                <img src="/images/simplesfa.png" alt="SFA Logo" style="width: 55%">
            </div>
            <div class="login-logo">
                <a href='<%= GetRouteUrl(AppModels.Routes.dashboard, null) %>'><b><%= AppModels.Company.appname %></b></a>
            </div>
            <!-- /.login-logo -->
            <div class="card">

                <asp:Panel runat="server" DefaultButton="btnsubmit">
                    <div class="card-body login-card-body">
                        <p class="login-box-msg">Sign in to start your session</p>

                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtusername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-user"></span>
                                </div>
                            </div>
                        </div>
                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtpassword" runat="server" CssClass="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-lock"></span>
                                </div>
                            </div>
                            <asp:Label ID="lblerror" runat="server" CssClass="text-error" Visible="false" Text="*Invalid Username or Password!"></asp:Label>
                        </div>
                        <div class="row">
                            <!-- /.col -->
                            <asp:Button ID="btnsubmit" runat="server" CssClass="btn btn-primary btn-block" CausesValidation="true" Text="Login" UseSubmitBehavior="false" OnClick="btnsubmit_Click" />
                            <!-- /.col -->
                        </div>
                    </div>

                </asp:Panel>
                <!-- /.login-card-body -->
            </div>
        </div>
    </form>
    <!-- /.login-box -->
    <!-- jQuery -->
    <%: Scripts.Render("~/bundles/jquery") %>
    <%: Scripts.Render("~/bundles/adminlte") %>
</body>
</html>
