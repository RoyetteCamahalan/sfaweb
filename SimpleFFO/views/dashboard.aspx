<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="SimpleFFO.views.dashboard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <div class="row" style="padding-top: 1rem;">
                    <div class="col-md-4 col-sm-4 col-12">
                        <div class="info-box">
                            <span class="info-box-icon bg-info"><i class="far fa-file-alt"></i></span>

                            <div class="info-box-content">
                                <span class="info-box-text">Drafts</span>
                                <span class="info-box-number">
                                    <asp:Literal ID="lblDraftCount" runat="server" Text="0"></asp:Literal>
                                </span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->
                    <div class="col-md-4 col-sm-4 col-12">
                        <div class="info-box">
                            <span class="info-box-icon bg-primary"><i class="far fa-paper-plane"></i></span>

                            <div class="info-box-content">
                                <span class="info-box-text">Submitted/Endorsed</span>
                                <span class="info-box-number">
                                    <asp:Literal ID="lblSubmittedCount" runat="server" Text="0"></asp:Literal>

                                </span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->
                    <div class="col-md-4 col-sm-4 col-12">
                        <div class="info-box">
                            <span class="info-box-icon bg-success"><i class="far fa-check-circle"></i></span>

                            <div class="info-box-content">
                                <span class="info-box-text">Approved</span>
                                <span class="info-box-number">
                                    <asp:Literal ID="lblApprovedCount" runat="server" Text="0"></asp:Literal>
                                </span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                        <!-- /.info-box -->
                    </div>
                </div>

                <asp:Panel ID="paneltoreview" runat="server" CssClass="row">
                    <div class="col-md-12">
                        <div class="card card-default">
                            <div class="card-header">
                                <h2 class="card-title">To Review</h2>
                            </div>
                            <div class="card-body">
                                <asp:GridView ID="dgvtoreview" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Convert.ToDateTime(Eval("date")).ToString("MM/dd/yyy") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PSR/PTR" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtemployee" runat="server" Text='<%# Eval("employeename") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Request Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtrequesttype" runat="server" Text='<%# Eval("modulename") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="txtDescription" runat="server" Text='<%# RequestDescription((GenericObject)Container.DataItem) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnaction" CssClass="btn btn-info btn-flat" runat="server" CausesValidation="false" CommandArgument='<%# Container.DataItemIndex %>' OnClick="btnrequestypelink_Click">Review</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="col-md-12">
                        <div class="card card-default">
                            <div class="card-header">
                                <h2 class="card-title">Request Summary
                                </h2>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-lg-12 form-group">
                                        <div class="btn-group float-right">
                                            <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                                                Create Request
                                            </button>
                                            <div class="dropdown-menu">
                                                <a class="dropdown-item" href='<%= GetRouteUrl(AppModels.Routes.requestpractivity, null) %>'>PR Activity</a>
                                                <a class="dropdown-item" href='<%= GetRouteUrl(AppModels.Routes.requeststop, null) %>'>S.T.O.P</a>
                                                <a class="dropdown-item" href='<%= GetRouteUrl(AppModels.Routes.requesttup, null) %>'>Tie-up Proposal</a>
                                                <div class="dropdown-divider"></div>
                                                <a class="dropdown-item" href='<%= GetRouteUrl(AppModels.Routes.requestloa, null) %>'>Leave of Absence</a>
                                            </div>
                                        </div>
                                        <div class="col-sm-3 float-right">

                                            <asp:DropDownList ID="cmbFilters" runat="server" CssClass="custom-select rounded-0">
                                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Activity" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="TUP" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="STOP" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <asp:GridView ID="tbl_main" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%" OnRowDataBound="tbl_main_RowDataBound">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Convert.ToDateTime(Eval("date")).ToString("MM/dd/yyy") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Request Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <a class="" href='<%# getElementRoute(Convert.ToInt32(Eval("moduleid")),Convert.ToInt64(Eval("id"))) %>'>
                                                    <%# Eval("modulename") %></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# RequestDescription((GenericObject)Container.DataItem) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtStatus" runat="server" CssClass='<%# getRequestStatusBadge((int)Eval("status")) %>' Text='<%# getRequestStatus((int)Eval("status")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status-Accounting" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtAccoutingStatus" runat="server">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div class="text-center">
                                                    <asp:LinkButton ID="btnview" CssClass="btn btn-info btn-sm btn-flat" runat="server" CausesValidation="false" CommandArgument='<%# Container.DataItemIndex %>' OnClick="btnaction_Click"> View </asp:LinkButton>
                                                    <asp:LinkButton ID="btnfundrequest" CssClass="btn btn-success btn-sm btn-flat" runat="server" CausesValidation="false" CommandArgument='<%# Container.DataItemIndex %>' Visible="false" OnClick="btnfundrequest_Click">Funding</asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>
     <%: Scripts.Render("~/bundles/fnsimple") %>
    <script type="text/javascript">  

        if ($.fn.dataTable.isDataTable('#MainContent_dgvtoreview')) {
            $('#MainContent_dgvtoreview').DataTable();
        }
        else {
            $('#MainContent_dgvtoreview').DataTable({
                "paging": true,
                "lengthChange": false,
                "searching": false,
                "ordering": false,
                "info": false,
                "autoWidth": true,
                "responsive": true,
            });
        }
        if ($.fn.dataTable.isDataTable('#MainContent_tbl_main')) {
            $('#MainContent_tbl_main').DataTable();
        }
        else {
            $('#MainContent_tbl_main').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": false,
                "info": true,
                "autoWidth": false,
                "responsive": true,
            });
        }
    </script>
</asp:Content>
