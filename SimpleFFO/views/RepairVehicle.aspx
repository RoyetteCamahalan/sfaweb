<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RepairVehicle.aspx.cs" Inherits="SimpleFFO.views.RepairVehicle" %>

<%@ Register TagName="StatusTrailComponent" Src="components/StatusTrailComponent.ascx" TagPrefix="uccom"%>
<%@ Register TagName="FundingComponent" Src="components/FundingComponent.ascx" TagPrefix="ucfunding"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
    <webopt:BundleReference runat="server" Path="~/Content/maincss" />

    <link href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/blitzer/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>
    <div class="content-wrapper">

        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Vehicle Repair</h1>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <section class="content">
            <div class="container-fluid">

                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Car Detail</h3>
                    </div>

                    <div class="card-body">

                        <div class="row">

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Employee</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtbox_fullname" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Company Car</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtbox_companycar" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        <span class="error invalid-feedback">No Vehicle was assigned to you. Request is not possible.</span>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Plate Number</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtbox_platenumber" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Year Model</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtbox_yearmodel" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>KM-Reading</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtbox_kmreading" runat="server" CssClass="form-control rounded-0" ReadOnly="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Date Filed</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtbox_datefiled" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <asp:Panel ID="paneldetails" runat="server" CssClass="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Repair Shop Detail</h3>
                    </div>

                    <div class="card-body">

                        <asp:UpdatePanel ID="UpdatePnlRepairDetail" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="panelshopheader" CssClass="row" runat="server">
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>
                                                <asp:Literal ID="lblrepairshops" runat="server" Text="Repair Shop *"></asp:Literal></label>
                                            <asp:ListBox ID="cmbRepairshop" runat="server" CssClass="select2" SelectionMode="Multiple" AutoPostBack="true" Width="100%" data-placeholder="--Select--"></asp:ListBox>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Button runat="server" ID="btnGenerate" CssClass="btn btn-primary btn-flat " Style="margin-top: 32px;"
                                                UseSubmitBehavior="false" CausesValidation="false" Text="Generate" OnClick="btnGenerate_Click" />

                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">

                                            <asp:Button runat="server" ID="btnAddNew" CssClass="btn btn-primary btn-flat float-right " Style="margin-top: 32px;"
                                                UseSubmitBehavior="false" CausesValidation="false" Text="Add New" OnClick="btnAddNew_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="tablecontainer" runat="server" Visible="false">
                                    <table id="tblvehiclerepairdetails" class="table table-bordered table-hover dataTable dtr-inline">
                                        <thead>
                                            <tr>
                                                <th style="width: 20%">NATURE OF REQUEST OR PARTS AND LABOR</th>
                                                <th style="width: 10%">QUANTITY</th>
                                                <asp:ListView ID="lstheader" runat="server">
                                                    <ItemTemplate>
                                                        <th class="text-center"><%# Eval("colheadername") %></th>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                                <% if (!this.isPageView)
                                                    { %>
                                                <th style="width: 5%"></th>

                                                <% } %>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:ListView ID="lstrowparticulars" runat="server" OnItemDataBound="lstrowparticulars_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblparticular" runat="server" Text='<%# Eval("reference") %>' Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtparticular" runat="server" CssClass="form-control rounded-0" Text='<%# Eval("reference") %>'></asp:TextBox>
                                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                                        </td>
                                                        <td class="text-center">
                                                            <asp:Label ID="lblquantity" runat="server" CssClass="text-center" Text='<%# Eval("qty") %>' Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control rounded-0 text-center" Text='<%# Eval("qty") %>'></asp:TextBox>
                                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.oneabove %></span>
                                                        </td>
                                                        <asp:ListView ID="lstsuppliers" runat="server" OnItemDataBound="lstsuppliers_ItemDataBound">
                                                            <ItemTemplate>
                                                                <td class="text-right">
                                                                    <asp:Label ID="lblprice" runat="server" CssClass="text-right" Text='<%# Convert.ToDecimal(Eval("price") ?? 1).ToString(AppModels.moneyformat) %>' Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtprice" runat="server" CssClass="form-control rounded-0 text-right"
                                                                        AutoPostBack="true" OnTextChanged="txtprice_TextChanged" Text='<%# Convert.ToDecimal(Eval("price") ?? 1).ToString(AppModels.moneyformat) %>'></asp:TextBox></td>
                                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.zeroabove %></span>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                        <% if (!this.isPageView)
                                                            { %>
                                                        <td style="width: 5%" class="text-center">
                                                            <asp:LinkButton ID="btnremove" runat="server" CssClass="btn btn-danger btn-sm td-center" CausesValidation="false" OnClick="btnremove_Click" CommandArgument='<%# Eval("rowid") %>'>
                                                                <i class="far fa-trash-alt"></i></asp:LinkButton></td>

                                                        <% } %>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </tbody>
                                        <tfoot>
                                            <tr>
                                                <td colspan="2" class="text-right">TOTAL</td>
                                                <asp:ListView ID="lstfooter" runat="server" OnItemDataBound="lstfooter_ItemDataBound">
                                                    <ItemTemplate>

                                                        <td>
                                                            <div>
                                                                <div class="d-flex">
                                                                    <asp:TextBox ID="txttotalprice" runat="server" CssClass="form-control rounded-0 text-right" Style="font-weight: 600" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                                <asp:Panel ID="panelchkselected" runat="server" Visible="false" CssClass="row mt-2 justify-content-center">
                                                                    <div class="icheck-primary d-inline">
                                                                        <asp:CheckBox ID="chkselected" CssClass="chkselected" runat="server" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkselected_CheckedChanged"/>
                                                                        <label for=".chkselected"><asp:Literal ID="lblchkselected" runat="server" Text="Choose Proposal"></asp:Literal></label>
                                                                    </div>
                                                                </asp:Panel>
                                                                
                                                            </div>
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </tr>
                                        </tfoot>
                                    </table>
                                </asp:Panel>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddNew" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />
                                <%--                                    <asp:AsyncPostBackTrigger ControlID="tblvehiclerepairdetails" EventName="TextChanged" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>              

                </asp:Panel>



            </div>
            
            <uccom:StatusTrailComponent ID="ctlStatusTrail" runat="server"></uccom:StatusTrailComponent>
            <ucfunding:FundingComponent ID="ctlFunding" runat="server"></ucfunding:FundingComponent>

            <div class="row">
                <div class="col-lg-12 form-group">
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-danger btn-flat float-right" UseSubmitBehavior="false" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary btn-flat float-right" CausesValidation="false" Text="Submit" OnClick="btnSubmit_Click" Style="margin-right: 5px;" />
                    <asp:Button runat="server" ID="btnTagasCancelled" CssClass="btn btn-danger btn-flat float-right" Text="Cancel Request" CausesValidation="false" OnClick="btnTagasCancelled_Click" Style="margin-right: 5px;" Visible="false" />
                </div>
            </div>
        </section>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>
    <%: Scripts.Render("~/bundles/jquery") %>

    <script>
        $(function () {
            $('.select2').select2()

            $('.select2b4').select2({
                theme: 'bootstrap4'
            })
        })

        function pageLoad() {
            $('.select2').select2()

            $('.select2b4').select2({
                theme: 'bootstrap4'
            })
        }
    </script>
</asp:Content>
