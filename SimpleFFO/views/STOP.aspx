<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="STOP.aspx.cs" Inherits="SimpleFFO.Views.STOP"
    MaintainScrollPositionOnPostback="true" SmartNavigation="true" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagName="StatusTrailComponent" Src="components/StatusTrailComponent.ascx" TagPrefix="uccom"%>
<%@ Register TagName="FundingComponent" Src="components/FundingComponent.ascx" TagPrefix="ucfunding"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Special Trade Outlet Promo</h1>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <section class="content">
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Customer Profile</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upanelcompanyinfo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label for="cmbCompany">Full Business Name <span class="required">*</span></label>
                                    <asp:DropDownList ID="cmbCompany" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true"
                                        OnSelectedIndexChanged="cmbCompany_SelectedIndexChanged" AppendDataBoundItems="true">
                                        <asp:ListItem Text="--Select--" Selected="True" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group">
                                            <label for="txtPurhaserName">Name of Purchaser </label>
                                            <asp:TextBox ID="txtPurhaserName" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label for="txtAvgPurhase">Ave. Monthly Purchase </label>
                                            <asp:TextBox ID="txtAvgPurhase" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtCompanyNotes">Additional Notes or Customer Information</label>
                                            <asp:TextBox ID="txtCompanyNotes" runat="server" CssClass="form-control rounded-0" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-sm-12 col-xs-12">

                                        <div class="form-group">
                                            <label for="txtNameofOwner">Name of Owner</label>
                                            <asp:TextBox ID="txtNameofOwner" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label for="txtContactPerson">Contact Person</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-user"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="txtContactNo">Contact Number</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-phone"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="txtNoofOutlets"># of outlets</label>
                                            <asp:TextBox ID="txtNoofOutlets" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbCompany" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>


            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Tie-Up Particulars</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upaneltieupdetails" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group">
                                            <label for="cmbTieUpMode">Tie-Up Mode</label>
                                            <asp:DropDownList ID="cmbTieUpMode" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Selected="True" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>

                                        <div class="form-group">
                                            <label for="cmbClaimType">Description and Specifics</label>
                                            <asp:DropDownList ID="cmbClaimType" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Selected="True" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtNotes">Notes on Tie-Up Particulars</label>
                                            <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control rounded-0" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group">
                                            <label for="txtTieupduration">Tie-Up Duration (in months) *</label>
                                            <asp:TextBox ID="txtTieupduration" runat="server" CssClass="form-control rounded-0" Text="1" AutoPostBack="true"
                                                TextMode="Number" OnTextChanged="dtpDate_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label>From <span class="required">*</span></label>
                                                    <div class="input-group date" data-target-input="nearest">
                                                        <asp:TextBox ID="dtpDate" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpDate"
                                                            aria-describedby="inputSuccess2Status3" OnTextChanged="dtpDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                        </div>
                                                        <span id="inputSuccess2Status3" class="sr-only">(success)</span>
                                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.invaliddate %></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <div class="form-group">
                                                    <label>To </label>
                                                    <div class="input-group date" data-target-input="nearest">
                                                        <asp:TextBox ID="txtDurationTo" runat="server" ReadOnly="true" CssClass="form-control datetimepicker-input rounded-0 txtDurationTo" aria-describedby="inputSuccess2Status4"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                        </div>
                                                        <span id="inputSuccess2Status4" class="sr-only">(success)</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtOtherPromoOffers">Other Promo Offers (i.e. raffle prizes, etc)</label>
                                            <asp:TextBox ID="txtOtherPromoOffers" runat="server" CssClass="form-control rounded-0" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtTieupduration" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="dtpDate" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Products Involved</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upanelproducts" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label for="cmbProductType">Product Type <span class="required">*</span></label>
                                    <asp:DropDownList ID="cmbProductType" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true"
                                        AutoPostBack="true" OnSelectedIndexChanged="cmbProductType_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Selected="True" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                </div>

                                <div class="form-group">
                                    <label for="txtDiscount">Discount *</label>
                                    <div class="form-group row">
                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="col-sm-6 form-control rounded-0" AutoPostBack="true"
                                            TextMode="Number" OnTextChanged="textbox_TextChange">20</asp:TextBox>
                                        <div class="col-sm-3">
                                            <div class="form-check">
                                                <asp:CheckBox ID="chkhaspinmoney" CssClass="form-check-input" runat="server" Checked="true"
                                                    AutoPostBack="true" OnCheckedChanged="chkhaspinmoney_CheckedChanged" />
                                                <label class="form-check-label">Has PIN Money?</label>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="form-group" style="display: flex">
                                    <div class="form-group checkbox" style="margin-left: 4rem; padding-left: 10px;">
                                    </div>
                                </div>
                                <asp:GridView ID="tbl_products" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline"
                                    CellSpacing="0"
                                    Width="100%"
                                    OnRowDataBound="tbl_products_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Products" HeaderStyle-Width="5%"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrdName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Packing" ItemStyle-Width="10%" FooterStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrdPacking" runat="server" Text="BOX OF 5 AMPS"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Company Price" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrice" runat="server" Text='<%# Convert.ToDecimal((Eval("price") ?? "0")).ToString(AppModels.moneyformat) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rebate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRebate" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quantity Projected" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQuantity" runat="server" Width="100%" TextMode="Number" Text='<%# Convert.ToInt32(Eval("monthlyqty"))==0 ? "" : Eval("monthlyqty") %>'
                                                    CssClass="form-control round-0" AutoPostBack="true" OnTextChanged="textbox_TextChange" Style="text-align: center">                                                                                                               
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Rebate" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalRebate" runat="server" Font-Bold="true"> </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sales Projection" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSaleProjection" runat="server" Font-Bold="true"> </asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pin Money | Total" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <div class="row">
                                                    <asp:TextBox ID="txtPinM" runat="server" ReadOnly="true" Width="49%"
                                                        CssClass="form-control round-0" Text='<%# Convert.ToDecimal((Eval("pinmoney") ?? "0")).ToString(AppModels.moneyformat) %>'></asp:TextBox>
                                                    <div style="width: 2%"></div>
                                                    <asp:TextBox ID="txtTotalPinM" runat="server" ReadOnly="true" Width="49%"
                                                        CssClass="form-control round-0"></asp:TextBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbProductType" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkhaspinmoney" EventName="CheckedChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpPnlTotalROI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="col-sm-6 float-right">
                                    <div class="form-group row">
                                        <label class="col-sm-6 col-form-label" for="textTotalProjection">Total Monthly Projection</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">₱
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="textTotalProjection" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-sm-6 col-form-label" for="textNumTieup">Number of Months (Tie-Up Duration)</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="far fa-calendar-check"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="textNumTieup" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6 col-form-label" for="textTotalROI">Total Return of Investment</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">₱
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="textTotalROI" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6 col-form-label" for="textTotalROI">Total Pin Money/Month</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">₱
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="textTotalPinM" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Computation of Rebates</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="UpPnlTotalRebate" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group row">
                                            <label class="col-sm-6 col-form-label" for="txtRebatesPerMonth">Total Rebates Projected per Month</label>
                                            <div class="col-sm-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">₱
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtRebatesPerMonth" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-6 col-form-label" for="txtNoTieup">Number of Months Tie-Up Duration</label>
                                            <div class="col-sm-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text"><i class="far fa-calendar-check"></i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtNoTieup" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-6 col-form-label" for="txtTotalRebatesforTieup">Total Rebates for Tie-Up Duration</label>
                                            <div class="col-sm-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">₱
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtTotalRebatesforTieup" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="txtRebateNotes">Notes on Computation of Rebates</label>
                                            <asp:TextBox ID="txtRebateNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <uccom:StatusTrailComponent ID="ctlStatusTrail" runat="server"></uccom:StatusTrailComponent>
            <ucfunding:FundingComponent ID="ctlFunding" runat="server"></ucfunding:FundingComponent>

            <div class="row">
                <div class="col-lg-12 form-group">
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-default btn-flat float-right" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
                    <asp:Button runat="server" ID="btnSaveDraft" CssClass="btn btn-primary btn-flat float-right" Style="margin-right: 5px;" CausesValidation="false" Text="Save Draft" OnClick="btnSaveDraft_Click" />
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px;" Text="Submit" CausesValidation="false" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>

    <script type="text/javascript">

        $(document).ready(function () {
            bindJs();
        });

        function bindJs() {
            if ($.fn.dataTable.isDataTable('#MainContent_tbl_products')) {
                $('#MainContent_tbl_products').DataTable();
            }
            else {
                $('#MainContent_tbl_products').DataTable({
                    "paging": false,
                    "lengthChange": false,
                    "searching": false,
                    "ordering": false,
                    "info": false,
                    "autoWidth": true,
                    "responsive": true,
                });
            }


            $('.dtpDate').daterangepicker({
                singleDatePicker: true,
                calender_style: "picker_3"
            }, function (start, end, label) {
                $('.dtpDate').trigger('change');
            });
        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            bindJs()
        });
    </script>
</asp:Content>
