<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MobileApps.aspx.cs" Inherits="SimpleFFO.views.MobileApps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

    <div class="content-wrapper">

        <section class="content-header">

            <div class="container-fluid">
            </div>
        </section>

        <section class="content">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h4>Mobile Apps</h4>
                    </div>
                </div>
                <div class="ml-4">
                    <div class="row">
                        <div class="col-sm-6">
                            <span>SFA for BBDM/RBDM (Android)</span>
                            <hr />
                        </div>
                        <div class="col-sm-4">
                            <asp:HyperLink ID="btndownloadsfadmlink" runat="server" NavigateUrl="~" CssClass="btn btn-sm btn-flat btn-primary">Download</asp:HyperLink>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <span>SFA for PSR/PTR (Android)</span>
                            <hr />
                        </div>
                        <div class="col-sm-4">
                            <asp:HyperLink ID="btndownloadsfalink" runat="server" NavigateUrl="~/Files/SFA/Apps/sfav1.4.9.210529_int.apk" CssClass="btn btn-sm btn-flat btn-primary">Download</asp:HyperLink>
                        </div>
                    </div>
                </div><div class="row mb-2">
                    <div class="col-sm-6">
                        <h4>User Manuals</h4>
                    </div>
                </div>
                <div class="ml-4">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:HyperLink NavigateUrl="~/Files/SFADM/manuals/SFADM_Guidev2.0.pdf" runat="server" Text="SFA for BBDM/RBDM" Target="_blank" />
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:HyperLink NavigateUrl="~/files/sfa/manuals/userguide.pdf" runat="server" Text="SFA for PSR/PTR" Target="_blank" />
                            <hr />
                        </div>
                    </div>
                </div>
            </div>
        </section>


    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
</asp:Content>
