<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorLogs.aspx.cs" Inherits="SimpleFFO.views.ErrorLogs" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Error Logs</h1>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <section class="content">
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"></h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upanelreportmain" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <asp:Panel ID="panelyear" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Year</label>
                                            <asp:DropDownList ID="cmbyear" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbyear_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelcycle" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Cycle</label>
                                            <asp:DropDownList ID="cmbcycle" runat="server" CssClass="custom-select rounded-0"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="row">
                                    <asp:Panel ID="panelrbdm" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Region/RBDM</label>
                                            <asp:DropDownList ID="cmbrbdm" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true"  OnSelectedIndexChanged="cmbrbdm_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelbranch" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Branch</label>
                                            <asp:DropDownList ID="cmbbranches" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbbranches_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelbbdm" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>BBDM</label>
                                            <asp:DropDownList ID="cmbbbdm" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbbbdm_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelpsr" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>PSR/PTR</label>
                                            <asp:DropDownList ID="cmbpsr" runat="server" CssClass="custom-select rounded-0"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12 form-group">
                                        <asp:Button ID="ButtonLoad" runat="server" Text=" Load Data" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px; display: none" OnClick="btnLoadData" />
                                        <asp:Button ID="btnloadinit" runat="server" Text=" Load Data" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px;" OnClick="btnloadinit_Click" />
                                        <%--<asp:Button ID="btnexportinit" runat="server" Text=" Export to Excel" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px;" OnClick="btnexportinit_Click" Visible="false"/>
                                        <asp:Button ID="btnexportfile" runat="server" Text=" Export to Excel" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px; display: none" OnClick="btnGenerateExcel" />--%>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbbranches" />
                                <asp:AsyncPostBackTrigger ControlID="cmbbbdm" />
                                <asp:AsyncPostBackTrigger ControlID="cmbyear" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonLoad" />
<%--                                <asp:AsyncPostBackTrigger ControlID="btnexportinit" />--%>
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="upanelgrids" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="panelpreviewunavailable" runat="server" CssClass="col-lg-12" Visible="false">
                                    <div class="text-center">
                                        <span>
                                            <asp:Literal ID="lblprevunavailable" runat="server"></asp:Literal></span>
                                    </div>
                                </asp:Panel>
                                <asp:ListView ID="tbl_main" runat="server" ItemPlaceholderID="itemPlaceHolder1" ViewStateMode="Disabled" EnableViewState="false">
                                    <LayoutTemplate>
                                        <table class="table table-bordered table-hover dataTable dtr-inline" id="table_callreportdetails">
                                            <thead>
                                                <th class="text-center" style="width: 25%">User</th>
                                                <th class="text-center" style="width: 20%">Module</th>
                                                <th class="text-center" style="width: 35%">Action</th>
                                                <th class="text-center" style="width: 20%">Log Date</th>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("employeename") %>'></asp:Label></td>
                                            <td class="text-center">
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("module") %>'></asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("action") %>'></asp:Label></td>
                                            <td class="text-center">
                                                <asp:Label ID="txtactualmcp" runat="server" Font-Size="Small" Text='<%# Convert.ToDateTime(Eval("logdate")).ToString("MM/dd/yyyy hh:mm tt") %>'></asp:Label></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="panelloader" class="overlay" style="display: none">
                        <i class="fas fa-2x fa-sync-alt fa-spin"></i>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>


</asp:Content>