<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TUP.aspx.cs" Inherits="SimpleFFO.Views.TUP"
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
                        <h1>Tie-Up Proposal</h1>
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
                                    <div class="col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="txtSpecialty">Specialty </label>
                                            <asp:TextBox ID="txtSpecialty" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label for="txtPractice">Practice </label>
                                            <asp:TextBox ID="txtPractice" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtPatientperDay">Average Patients per Day</label>
                                            <asp:TextBox ID="txtPatientperDay" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtCompanyNotes">Additional Notes or Customer Information</label>
                                            <asp:TextBox ID="txtCompanyNotes" runat="server" CssClass="form-control rounded-0" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="txtclinicAddress">Clinic Address</label>
                                            <asp:TextBox ID="txtclinicAddress" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtClinicSchedule">Clinic Schedule</label>
                                            <asp:TextBox ID="txtClinicSchedule" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label for="txtPatientType">Patient Type</label>
                                            <asp:TextBox ID="txtPatientType" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
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
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbCompany" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>


                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Tie-Up Particulars</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upaneltieupdetails" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <div class="row">

                                    <div class="col-sm-6 col-xs-12">
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
                                            <label for="cmbTieupType">Type of Tie-Up</label>
                                            <asp:DropDownList ID="cmbTieupType" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Selected="True" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtNotes">Notes on Tie-Up Particulars</label>
                                            <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control rounded-0" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="txtTieupduration">Tie-Up Duration (in months) *</label>
                                            <asp:TextBox ID="txtTieupduration" runat="server" CssClass="form-control rounded-0" Text="1" AutoPostBack="true"
                                                TextMode="Number" OnTextChanged="dtpDate_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-12">
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
                                            <div class="col-sm-6 col-xs-12">
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
                                            <label for="txtDespensingCustomer">Dispensing Customer </label>
                                            <asp:TextBox ID="txtDespensingCustomer" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtTradeOutlet">Trade Outlet (nearby drugstore) </label>
                                            <asp:TextBox ID="txtTradeOutlet" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtHospitalPharmacy">Hospital Pharmacy</label>
                                            <asp:TextBox ID="txtHospitalPharmacy" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
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


                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Products Involved</h3>
                    </div>
                    <div class="card-body">

                        <asp:UpdatePanel ID="upanelproducts" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label for="cmbProductType">Product Type<span class="required">*</span></label>
                                    <asp:DropDownList ID="cmbProductType" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true"
                                        AutoPostBack="true" OnSelectedIndexChanged="cmbProductType_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Selected="True" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                </div>
                                <asp:GridView ID="tbl_products" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline"
                                    OnRowDataBound="tbl_products_RowDataBound"
                                    CellSpacing="0"
                                    ShowFooter="true"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Products" HeaderStyle-Width="5%"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrdName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Packing" HeaderStyle-Width="5%" FooterStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrdPacking" runat="server" Text="BOX OF 5 AMPS"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Company Price" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrice" runat="server" Text='<%# Convert.ToDecimal((Eval("price") ?? "0")).ToString(AppModels.moneyformat) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quantity Projected" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQuantity" runat="server" Width="100%" TextMode="Number" Text='<%# Convert.ToInt32(Eval("monthlyqty"))==0 ? "" : Eval("monthlyqty") %>'
                                                    CssClass="form-control rounded-0" AutoPostBack="true" OnTextChanged="textbox_TextChange" Style="text-align: center">                                                                                                               
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sales Projection" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSaleProjection" runat="server" Font-Bold="true"> </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Discount / Net Price" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <div class="row">
                                                    <asp:TextBox ID="txtItemDiscount" runat="server" Width="49%"
                                                        CssClass="form-control text-center" AutoPostBack="true" OnTextChanged="textbox_TextChange" Text='<%# Convert.ToDecimal(Eval("discount"))==0 ? "" : Eval("discount") %>'></asp:TextBox>
                                                    <div style="width: 2%"></div>
                                                    <asp:TextBox ID="txtNetPrice" runat="server" ReadOnly="true" Width="49%"
                                                        CssClass="form-control text-center"></asp:TextBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbProductType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpPnlTotalROI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="col-sm-6 float-right">
                                    <div class="form-group row">
                                        <label class="col-sm-6 col-form-label" for="txtTotalMonthlySales">Total Monthly Projection</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">₱
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtTotalMonthlySales" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                <span class="error invalid-feedback">*Product Required</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-sm-6 col-form-label" for="txtTieUpDuration2">Number of Months (Tie-Up Duration)</label>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="far fa-calendar-check"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtTieUpDuration2" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
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
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>


                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Computation of Rebates</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="UpPnlTotalRebate" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-sm-6 col-xs-12">
                                        <div class="form-group row">
                                            <label class="col-sm-6 col-form-label" for="textTotalMonthlyProjection">Product Projection Total</label>
                                            <div class="col-sm-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">₱
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="textTotalMonthlyProjection" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-6 col-form-label" for="textTotalMonthlyProjection">Net of Product Rebate</label>
                                            <div class="col-sm-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">₱
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtNetProductRebate" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-6 col-form-label" for="textTotalMonthlyProjection">Net Monthly Rebate</label>
                                            <div class="col-sm-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">₱
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtTotalMonthlyRebate" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-6 col-form-label" for="txtTieUpDuration3">Number of Months Tie-Up Duration</label>
                                            <div class="col-sm-6">
                                                <div class="input-group">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text"><i class="far fa-calendar-check"></i>
                                                        </span>
                                                    </div>
                                                    <asp:TextBox ID="txtTieUpDuration3" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
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
                                            <asp:TextBox ID="txtRebateNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
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
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px;" CausesValidation="false" Text="Submit" OnClick="btnSubmit_Click" />
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
                    "paging": false,
                    "lengthChange": false,
                    "searching": false,
                    "ordering": false,
                    "info": false,
                    "autoWidth": false,
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
